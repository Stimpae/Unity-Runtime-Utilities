using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using static System.Environment;
using static System.IO.Path;
using static UnityEditor.AssetDatabase;

// Taken from adammyhre's Gist, with some modifications to suit my needs
// https://gist.github.com/adammyhre/ce4009edccc420a35237419b5ea050e1

namespace TG.Utilities {
    public static class ProjectSetup {
        [MenuItem("Tools/Setup/Import Essential Assets")]
        public static void ImportEssentials() {
            Assets.ImportAsset("Hot Reload Edit Code Without Compiling.unitypackage", "The Naughty Cult/Editor ExtensionsUtilities");
            Assets.ImportAsset("Easy Save - The Complete Save Data Serializer System.unitypackage", "Moodkie/Editor ExtensionsUtilities");
            Assets.ImportAsset("Pro Camera 2D - The definitive 2D 25D camera plugin for Unity.unitypackage","Lus Pedro Fonseca/2D");
            Assets.ImportAsset("Serialized Dictionary.unitypackage", "ayellowpaper/Editor ExtensionsUtilities");
            Assets.ImportAsset("FMOD for Unity.unitypackage", "FMOD/Editor ExtensionsAudio");
        }

        [MenuItem("Tools/Setup/Install Essential Packages")]
        public static void InstallPackages() {
            Packages.InstallPackages(new[] {
                // just comment out the tg packages you dont need.
                "git+https://github.com/starikcetin/Eflatun.SceneReference.git#4.1.1",
                "git+https://github.com/Stimpae/TG-Unity-Editor-Toolkit.git",
                "git+https://github.com/Stimpae/TG-Unity-Utilities.git",
                "git+https://github.com/Stimpae/TG-Unity-Game-Management.git",
                "git+https://github.com/Stimpae/TG-Unity-UI-Management.git",
                "git+https://github.com/Stimpae/TG-Game-Feel.git",
                "git+https://github.com/Stimpae/TG-Tween.git",
            });
        }

        [MenuItem("Tools/Setup/Create Folders")]
        public static void CreateFolders() {
            Folders.Create("", 
                "Editor", "Fonts", "Materials", "Plugins", "Prefabs", "Resources", "Scripts", "Settings", "Shaders", "Sprites");
            Refresh();
            
            MoveAsset("Assets/InputSystem_Actions.inputactions", "Assets/Settings/InputSystem_Actions.inputactions");
            MoveAsset("Assets/DefaultVolumeProfile.asset", "Assets/Settings/DefaultVolumeProfile.asset");
           
            Refresh();
        }

        private static class Assets {
            public static void ImportAsset(string asset, string folder) {
                string basePath;
                if (OSVersion.Platform is PlatformID.MacOSX or PlatformID.Unix) {
                    string homeDirectory = GetFolderPath(SpecialFolder.Personal);
                    basePath = Combine(homeDirectory, "Library/Unity/Asset Store-5.x");
                }
                else {
                    string defaultPath = Combine(GetFolderPath(SpecialFolder.ApplicationData), "Unity");
                    basePath = Combine(EditorPrefs.GetString("AssetStoreCacheRootPath", defaultPath),
                        "Asset Store-5.x");
                }

                asset = asset.EndsWith(".unitypackage") ? asset : asset + ".unitypackage";

                string fullPath = Combine(basePath, folder, asset);

                if (!File.Exists(fullPath)) {
                    throw new FileNotFoundException($"The asset package was not found at the path: {fullPath}");
                }

                ImportPackage(fullPath, false);
            }
        }

        private static class Packages {
            static AddRequest _request;
            static Queue<string> _packagesToInstall = new Queue<string>();

            public static void InstallPackages(string[] packages) {
                foreach (var package in packages) {
                    _packagesToInstall.Enqueue(package);
                }

                if (_packagesToInstall.Count > 0) {
                    StartNextPackageInstallation();
                }
            }

            static async void StartNextPackageInstallation() {
                _request = Client.Add(_packagesToInstall.Dequeue());

                while (!_request.IsCompleted) await Task.Delay(10);

                if (_request.Status == StatusCode.Success) Debug.Log("Installed: " + _request.Result.packageId);
                else if (_request.Status >= StatusCode.Failure) Debug.LogError(_request.Error.message);

                if (_packagesToInstall.Count > 0) {
                    await Task.Delay(1000);
                    StartNextPackageInstallation();
                }
            }
        }

        private static class Folders {
            public static void Create(string root, params string[] folders) {
                var fullpath = Combine(Application.dataPath, root);
                if (!Directory.Exists(fullpath)) {
                    Directory.CreateDirectory(fullpath);
                }

                foreach (var folder in folders) {
                    CreateSubFolders(fullpath, folder);
                }
            }

            static void CreateSubFolders(string rootPath, string folderHierarchy) {
                var folders = folderHierarchy.Split('/');
                var currentPath = rootPath;

                foreach (var folder in folders) {
                    currentPath = Combine(currentPath, folder);
                    if (!Directory.Exists(currentPath)) {
                        Directory.CreateDirectory(currentPath);
                    }
                }
            }

            public static void Move(string newParent, string folderName) {
                var sourcePath = $"Assets/{folderName}";
                if (IsValidFolder(sourcePath)) {
                    var destinationPath = $"Assets/{newParent}/{folderName}";
                    var error = MoveAsset(sourcePath, destinationPath);

                    if (!string.IsNullOrEmpty(error)) {
                        Debug.LogError($"Failed to move {folderName}: {error}");
                    }
                }
            }

            public static void Delete(string folderName) {
                var pathToDelete = $"Assets/{folderName}";

                if (IsValidFolder(pathToDelete)) {
                    DeleteAsset(pathToDelete);
                }
            }
        }
    }
}