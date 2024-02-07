using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LS.Automation.SceneAutomation
{
    public class SceneAutomation : EditorWindow
    {
        private enum SceneType { Empty, _2D, _2DUI, _3D }
        private SceneType sceneType;
        private string sceneName = "NewScene";
        private GameObject root;

        [MenuItem("Automation Tools/Productivity/Scene Creator %&n")] // Ctrl+Alt+N
        private static void Init()
        {
            SceneAutomation window = (SceneAutomation)EditorWindow.GetWindow(typeof(SceneAutomation));
            window.minSize = new Vector2(300, 150);  // Set the minimum size
            window.maxSize = new Vector2(300, 150);  // Set the maximum size
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Label("Create New Scene", EditorStyles.boldLabel);

            sceneType = (SceneType)EditorGUILayout.EnumPopup("Scene Type", sceneType);
            sceneName = EditorGUILayout.TextField("Scene Name", sceneName);

            if (GUILayout.Button("Create Scene"))
            {
                if (string.IsNullOrEmpty(sceneName))
                {
                    Debug.LogError("Scene name cannot be empty.");
                    return;
                }

                EditorApplication.isPlaying = false;

                switch (sceneType)
                {
                    case SceneType.Empty:
                        CreateEmptyScene();
                        break;
                    case SceneType._2D:
                        Create2DScene();
                        break;
                    case SceneType._2DUI:
                        Create2DUIScene();
                        break;
                    case SceneType._3D:
                        Create3DScene();
                        break;
                    default:
                        Debug.LogError("Invalid scene type.");
                        break;
                }
                Close();
            }
        }

        private void CreateEmptyScene()
        {
            if (!PromptToChangeSceneName()) return;

            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            CreateRoot();
        }

        private void Create2DScene()
        {
            if (!PromptToChangeSceneName()) return;

            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
            RemoveDefaultObjects();
            CreateRoot();

            EditorApplication.delayCall += () =>
            {
                CreateCameraHierarchy("Cameras", true);
                CreateGameplayElements();
                CreateEnvironment();
                CreateManagers();
                CreateMiscellaneous();
                CreateCanvasHierarchy("Canvases", true);
                CreateEventSystem();
                CreateDirectionalLight("Directional Light");

                // Save the scene after creation
                SaveScene();
            };
        }

        private void Create2DUIScene()
        {
            if (!PromptToChangeSceneName()) return;

            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
            RemoveDefaultObjects();
            CreateRoot();

            EditorApplication.delayCall += () =>
            {
                CreateCameraHierarchy("Cameras", true);
                CreateManagers();
                CreateMiscellaneous();
                CreateCanvasHierarchy("Canvases", false);
                CreateEventSystem();
                CreateDirectionalLight("Directional Light");

                // Save the scene after creation
                SaveScene();
            };
        }

        private void Create3DScene()
        {
            if (!PromptToChangeSceneName()) return;

            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
            RemoveDefaultObjects();
            CreateRoot();

            CreateCameraHierarchy("Cameras", false);
            CreateGameplayElements();
            CreateEnvironment();
            CreateManagers();
            CreateMiscellaneous();
            CreateDirectionalLight("Directional Light");

            // Save the scene after creation
            SaveScene();
        }

        private void CreateGameplayElements()
        {
            if (root == null)
            {
                Debug.LogError("Root GameObject not found.");
                return;
            }

            GameObject gameplayElementsObject = new GameObject("Gameplay Elements");
            gameplayElementsObject.transform.SetParent(root.transform);
            CreateSubObject("Player", gameplayElementsObject);
            CreateSubObject("Enemies", gameplayElementsObject);
            CreateSubObject("NPCs", gameplayElementsObject);
            CreateSubObject("Items", gameplayElementsObject);
        }

        private void CreateEnvironment()
        {
            if (root == null)
            {
                Debug.LogError("Root GameObject not found.");
                return;
            }

            GameObject environmentObject = new GameObject("Environment");
            environmentObject.transform.SetParent(root.transform);
            CreateSubObject("Terrain", environmentObject);
            CreateSubObject("Obstacles", environmentObject);
            CreateSubObject("InteractiveObjects", environmentObject);
        }

        private void CreateSubObject(string subObjectName, GameObject parentObject)
        {
            GameObject subObject = new GameObject(subObjectName);
            subObject.transform.SetParent(parentObject.transform);
        }

        private void CreateManagers()
        {
            if (root == null)
            {
                Debug.LogError("Root GameObject not found.");
                return;
            }

            GameObject managersObject = new GameObject("Managers");
            managersObject.transform.SetParent(root.transform);
            CreateSubObject("GameManager", managersObject);
            CreateSubObject("UIManager", managersObject);
            CreateSubObject("AudioManager", managersObject);
            CreateSubObject("InputManager", managersObject);
        }

        private void CreateMiscellaneous()
        {
            if (root == null)
            {
                Debug.LogError("Root GameObject not found.");
                return;
            }

            GameObject miscellaneousObject = new GameObject("Miscellaneous");
            miscellaneousObject.transform.SetParent(root.transform);
            CreateSubObject("Event Triggers", miscellaneousObject);
            CreateSubObject("System Managers", miscellaneousObject);
        }

        private void CreateDirectionalLight(string lightName)
        {
            if (root == null)
            {
                Debug.LogError("Root GameObject not found.");
                return;
            }

            GameObject existingDirectionalLight = GameObject.Find(lightName);
            if (existingDirectionalLight != null && existingDirectionalLight.transform.parent == null)
            {
                DestroyImmediate(existingDirectionalLight);
            }

            GameObject lightObject = new GameObject(lightName);
            Light directionalLight = lightObject.AddComponent<Light>();
            directionalLight.type = LightType.Directional;
            lightObject.transform.rotation = Quaternion.Euler(new Vector3(50f, -30f, 0f));
            lightObject.transform.SetParent(root.transform);
        }

        private void CreateCameraHierarchy(string parentName, bool is2D)
        {
            if (root == null)
            {
                Debug.LogError("Root GameObject not found.");
                return;
            }

            GameObject camerasObject = new GameObject(parentName);
            camerasObject.transform.SetParent(root.transform);
            GameObject cameraObject = new GameObject("Main Camera");
            Camera camera = cameraObject.AddComponent<Camera>();

            if (is2D)
            {
                camera.orthographic = true;
                camera.clearFlags = CameraClearFlags.SolidColor;
                camera.backgroundColor = Color.black;
                cameraObject.transform.SetParent(camerasObject.transform);
            }
            else
            {
                camera.clearFlags = CameraClearFlags.SolidColor;
                camera.backgroundColor = Color.black;
                cameraObject.transform.SetParent(camerasObject.transform);
                cameraObject.transform.position = new Vector3(0f, 1.8f, -5f);
                cameraObject.transform.rotation = Quaternion.Euler(new Vector3(30f, 0f, 0f));
            }
        }

        private void CreateEventSystem()
        {
            if (root == null)
            {
                Debug.LogError("Root GameObject not found.");
                return;
            }

            new GameObject("EventSystem", typeof(UnityEngine.EventSystems.EventSystem), typeof(UnityEngine.EventSystems.StandaloneInputModule)).transform.SetParent(root.transform);
        }

        private void CreateRoot()
        {
            root = new GameObject("Root");
        }

        private void CreateCanvasHierarchy(string parentName, bool is2D)
        {
            if (root == null)
            {
                Debug.LogError("Root GameObject not found.");
                return;
            }

            GameObject canvasesObject = new GameObject(parentName);
            canvasesObject.transform.SetParent(root.transform);

            GameObject staticCanvasesObject = new GameObject("Static Canvases");
            staticCanvasesObject.transform.SetParent(canvasesObject.transform);
            CreateCanvas("Main Canvas", staticCanvasesObject, is2D);

            GameObject dynamicCanvasesObject = new GameObject("Dynamic Canvases");
            dynamicCanvasesObject.transform.SetParent(canvasesObject.transform);
        }

        private void CreateCanvas(string canvasName, GameObject parentObject, bool is2D)
        {
            GameObject canvasObject = new GameObject(canvasName);
            Canvas canvas = canvasObject.AddComponent<Canvas>();

            canvas.renderMode = is2D ? RenderMode.ScreenSpaceCamera : RenderMode.ScreenSpaceOverlay;

            CanvasScaler canvasScaler = canvasObject.AddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(2048, 2732); // iPad resolution

            canvasObject.AddComponent<GraphicRaycaster>();

            if (is2D)
            {
                Camera mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
                canvas.worldCamera = mainCamera;
            }

            canvasObject.transform.SetParent(parentObject.transform);
        }

        private void SaveScene()
        {
            string scenesFolderPath = "Assets/Scenes/";
            if (!AssetDatabase.IsValidFolder(scenesFolderPath))
            {
                AssetDatabase.CreateFolder("Assets", "Scenes");
            }

            string scenePath = scenesFolderPath + sceneName + ".unity";
            EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), scenePath);
        }

        private void RemoveDefaultObjects()
        {
            GameObject[] defaultObjects = GameObject.FindObjectsOfType<GameObject>();
            foreach (GameObject obj in defaultObjects)
            {
                if (obj.GetComponent<Camera>() != null)
                {
                    DestroyImmediate(obj);
                }
            }
        }

        private bool PromptToChangeSceneName()
        {
            string scenePath = "Assets/Scenes/" + sceneName + ".unity";
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath) != null)
            {
                return EditorUtility.DisplayDialog("Scene Already Exists", "A scene with the name '" + sceneName + "' already exists. Do you want to overwrite it?", "Yes", "No");
            }
            return true;
        }
    }
}
