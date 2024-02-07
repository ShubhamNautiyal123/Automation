using UnityEditor;
using UnityEngine;
using System.IO;

namespace LS.Automation.StructureAutomation
{
    public class StructureAutomation : Editor
    {
        [MenuItem("Automation Tools/Productivity/Folder Structure Builder %&s")] // Ctrl+Alt+S
        private static void CreateProjectFolderStructure()
        {
            bool is2D = EditorUtility.DisplayDialog("Select Project Type", "Is this a 2D or 3D project?", "2D", "3D");

            if (is2D)
            {
                Create2DFolderStructure();
            }
            else
            {
                Create3DFolderStructure();
            }

            Debug.Log("Project folder structure created successfully!");
        }

        private static void Create2DFolderStructure()
        {
            CreateFolderIfNotExists("Assets/Scenes");
            CreateFolderIfNotExists("Assets/Scripts");
            CreateFolderIfNotExists("Assets/Scripts/UI");
            CreateFolderIfNotExists("Assets/Scripts/Controllers");
            CreateFolderIfNotExists("Assets/Scripts/Managers");
            CreateFolderIfNotExists("Assets/Scripts/Items");
            CreateFolderIfNotExists("Assets/Scripts/Utilities");
            CreateFolderIfNotExists("Assets/Scripts/Networking");
            CreateFolderIfNotExists("Assets/Scripts/Data");
            CreateFolderIfNotExists("Assets/Scripts/Input");
            CreateFolderIfNotExists("Assets/Sprites");
            CreateFolderIfNotExists("Assets/Animations");
            CreateFolderIfNotExists("Assets/Materials");
            CreateFolderIfNotExists("Assets/Prefabs");
            CreateFolderIfNotExists("Assets/Audio");
            CreateFolderIfNotExists("Assets/UI");
            CreateFolderIfNotExists("Assets/Fonts");
            CreateFolderIfNotExists("Assets/Textures");
            CreateFolderIfNotExists("Assets/Tilemaps");
            CreateFolderIfNotExists("Assets/PhysicsMaterials2D");
            CreateFolderIfNotExists("Assets/Editor");
            CreateFolderIfNotExists("Assets/Resources");
            CreateFolderIfNotExists("Assets/Editor Default Resources");
            CreateFolderIfNotExists("Assets/EditorIcons");
            CreateFolderIfNotExists("Assets/Gizmos");
            CreateFolderIfNotExists("Assets/StreamingAssets");
            CreateFolderIfNotExists("Assets/Plugins");
            CreateFolderIfNotExists("Assets/Shaders");
            CreateFolderIfNotExists("Assets/NavMesh");
            CreateFolderIfNotExists("Assets/Packages");
            CreateFolderIfNotExists("Assets/Standard Assets");

            Debug.Log("2D Project folder structure created successfully!");
        }

        private static void Create3DFolderStructure()
        {
            CreateFolderIfNotExists("Assets/Scenes");
            CreateFolderIfNotExists("Assets/Scripts");
            CreateFolderIfNotExists("Assets/Scripts/UI");
            CreateFolderIfNotExists("Assets/Scripts/Controllers");
            CreateFolderIfNotExists("Assets/Scripts/Managers");
            CreateFolderIfNotExists("Assets/Scripts/Items");
            CreateFolderIfNotExists("Assets/Scripts/Utilities");
            CreateFolderIfNotExists("Assets/Scripts/Networking");
            CreateFolderIfNotExists("Assets/Scripts/Data");
            CreateFolderIfNotExists("Assets/Scripts/Input");
            CreateFolderIfNotExists("Assets/Models");
            CreateFolderIfNotExists("Assets/Materials");
            CreateFolderIfNotExists("Assets/Textures");
            CreateFolderIfNotExists("Assets/Animations");
            CreateFolderIfNotExists("Assets/Prefabs");
            CreateFolderIfNotExists("Assets/Audio");
            CreateFolderIfNotExists("Assets/UI");
            CreateFolderIfNotExists("Assets/Fonts");
            CreateFolderIfNotExists("Assets/PhysicsMaterials");
            CreateFolderIfNotExists("Assets/Editor");
            CreateFolderIfNotExists("Assets/Resources");
            CreateFolderIfNotExists("Assets/Editor Default Resources");
            CreateFolderIfNotExists("Assets/EditorIcons");
            CreateFolderIfNotExists("Assets/Gizmos");
            CreateFolderIfNotExists("Assets/StreamingAssets");
            CreateFolderIfNotExists("Assets/Plugins");
            CreateFolderIfNotExists("Assets/Shaders");
            CreateFolderIfNotExists("Assets/NavMesh");
            CreateFolderIfNotExists("Assets/Packages");
            CreateFolderIfNotExists("Assets/Standard Assets");

            Debug.Log("3D Project folder structure created successfully!");
        }

        private static void CreateFolderIfNotExists(string path)
        {
            if (!AssetDatabase.IsValidFolder(path))
            {
                AssetDatabase.CreateFolder(Path.GetDirectoryName(path), Path.GetFileName(path));
            }
        }
    }
}
