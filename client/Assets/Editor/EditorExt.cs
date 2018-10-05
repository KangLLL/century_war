using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

using System.Reflection;

public class EditorExt
{
	[MenuItem("Ext/AddSurfaceNode")]
	public static void AddSurfaceNode()
	{
		Object[] selections = Selection.objects;
		
		foreach(Object p in selections)
		{
			if(p is GameObject)
			{
				GameObject go = p as GameObject;
				BuildingSurfaceBehavior surface =  go.GetComponent<BuildingSurfaceBehavior>();
				if(surface != null)
				{
					Transform s = go.transform.GetChild(0).FindChild("Surface");
					if(s != null)
					{
						GameObject.DestroyImmediate(s.gameObject);
					}
					
					GameObject empty = new GameObject();
					empty.name = "Surface";
					empty.transform.parent = go.transform.GetChild(0);
					empty.transform.localPosition = new Vector3(0,0,160);
					
					FieldInfo surfaceInfo = typeof(BuildingSurfaceBehavior).GetField("m_SurfaceParent",
						BindingFlags.Instance | BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.ExactBinding);
					surfaceInfo.SetValue(surface, empty.transform);
				}
			}
		}
	}
	
	[MenuItem("Ext/ModifyDoNotDestoryObject")]
	public static void ModifyDoNotDestoryObject()
	{
		Object[] selections = Selection.objects;
		
		foreach(Object p in selections)
		{
			if(p is GameObject)
			{
				GameObject go = p as GameObject;
				HPBehavior hp =  go.GetComponent<HPBehavior>();
				if(hp != null)
				{
					Transform s = go.transform.GetChild(0).FindChild("Surface");
					if(s != null)
					{
						FieldInfo notDestroyInfo = typeof(HPBehavior).GetField("m_DoNotDestoryGameObjects",
							BindingFlags.Instance | BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.ExactBinding);
						if(notDestroyInfo != null && notDestroyInfo.FieldType == typeof(GameObject[]))
						{
							GameObject[] ee = (GameObject[])notDestroyInfo.GetValue(hp);
							ee[0] = s.gameObject;
						}
						
					}
				}
			}
		}
	}
	
    [MenuItem("Ext/MultiApply")]
    public static void MultiApply()
    {
        Object[] selections = Selection.objects;
        List<GameObject> selectedGameObjectList = new List<GameObject>();
        foreach (Object obj in selections)
        {
            if (obj is GameObject)
            {
                GameObject rootGameObject = PrefabUtility.FindValidUploadPrefabInstanceRoot((GameObject)obj);
                if (rootGameObject != null && !selectedGameObjectList.Contains(rootGameObject))
                {
                    selectedGameObjectList.Add(rootGameObject);
                }
            }
        }
        foreach (GameObject root in selectedGameObjectList)
        {
            GameObject prefab = PrefabUtility.GetPrefabParent(root) as GameObject;
            PrefabUtility.ReplacePrefab(root, prefab, ReplacePrefabOptions.ConnectToPrefab);
        }
    }

	/*
    [MenuItem("Ext/CreateSpriteCollectionAndAnimation &j")]
    public static void CreateSpriteCollectionAndAnimation()
    {
        tk2dSpriteCollectionEditor.DoCollectionCreate();
        tk2dSpriteAnimationEditor.DoAnimationCreate();
    }
    */

    [MenuItem("Ext/AddFolderName &k")]
    public static void AddFolderName()
    {
        Object[] objs = Selection.objects;
        foreach (Object obj in objs)
        {
            if (obj.GetType().Name == "GameObject")
            {
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(AssetDatabase.GetAssetPath(obj));
                System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(fileInfo.DirectoryName);
                AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(obj), dirInfo.Name + fileInfo.Name.Remove(fileInfo.Name.LastIndexOf(fileInfo.Extension)));
            }
        }
     }

    [MenuItem("Ext/ChangeArmycamp")]
    public static void ChangeArmycamp()
    {
        Object[] selections = Selection.objects;
        List<GameObject> selectedGameObjectList = new List<GameObject>();
        foreach (Object obj in selections)
        {
            if (obj is GameObject)
            {
                GameObject rootGameObject = PrefabUtility.FindValidUploadPrefabInstanceRoot((GameObject)obj);
                if (rootGameObject != null && !selectedGameObjectList.Contains(rootGameObject))
                {
                    selectedGameObjectList.Add(rootGameObject);
                }
            }
        }
        foreach (GameObject root in selectedGameObjectList)
        {
            GameObject buildingBackground = root.transform.FindChild("BuildingBackgroundAnchor").FindChild("BuildingBackground").gameObject;
            Object.DestroyImmediate(buildingBackground.GetComponent<tk2dSprite>());
            tk2dSpriteAnimation spriteAnimation = null;
            tk2dGenericIndexItem[] items = tk2dEditorUtility.GetOrCreateIndex().GetSpriteAnimations();
            foreach (tk2dGenericIndexItem item in items)
            {

                if (item.AssetName == root.name + "SpriteAnimation")
                {
                    spriteAnimation = item.GetAsset<tk2dSpriteAnimation>();
                }
            }
            tk2dAnimatedSprite.AddComponent(buildingBackground, spriteAnimation, 0);
        }
    }

    [MenuItem("Ext/GenerateSoldierSpriteAnimationNoAttack")]
    public static void GenerateSoldierSpriteAnimationNoAttack()
    {
        tk2dSpriteAnimation spriteAnimation = Selection.activeGameObject.GetComponent<tk2dSpriteAnimation>();
        string name = spriteAnimation.name.Replace("SpriteAnimation", string.Empty);
        string spriteCollectionName = name + "SpriteCollection";
        tk2dSpriteCollectionData sc = null;

        tk2dSpriteCollectionIndex[] items = tk2dEditorUtility.GetOrCreateIndex().GetSpriteCollectionIndex();
        foreach (tk2dSpriteCollectionIndex item in items)
        {
            if (spriteCollectionName == item.name)
            {
                GameObject scgo = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(item.spriteCollectionDataGUID), typeof(GameObject)) as GameObject;
                sc = scgo.GetComponent<tk2dSpriteCollectionData>();
                break;
            }
        }

        if (spriteAnimation != null)
        {
            spriteAnimation.clips[0].name = "IdleDown";

            spriteAnimation.clips[0].wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[0].frames.Length; i++)
                {
                    spriteAnimation.clips[0].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[0].frames[frameNumber].spriteId = sc.GetSpriteIdByName("IdleDown" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[1].name = "IdleLeft";

            spriteAnimation.clips[1].wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[1].frames.Length; i++)
                {
                    spriteAnimation.clips[1].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[1].frames[frameNumber].spriteId = sc.GetSpriteIdByName("IdleLeft" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[2].name = "IdleLeftDown";

            spriteAnimation.clips[2].wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[2].frames.Length; i++)
                {
                    spriteAnimation.clips[2].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[2].frames[frameNumber].spriteId = sc.GetSpriteIdByName("IdleLeftDown" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[3].name = "IdleLeftUp";

            spriteAnimation.clips[3].wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[3].frames.Length; i++)
                {
                    spriteAnimation.clips[3].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[3].frames[frameNumber].spriteId = sc.GetSpriteIdByName("IdleLeftUp" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[4].name = "IdleRight";

            spriteAnimation.clips[4].wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[4].frames.Length; i++)
                {
                    spriteAnimation.clips[4].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[4].frames[frameNumber].spriteId = sc.GetSpriteIdByName("IdleRight" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[5].name = "IdleRightDown";

            spriteAnimation.clips[5].wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[5].frames.Length; i++)
                {
                    spriteAnimation.clips[5].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[5].frames[frameNumber].spriteId = sc.GetSpriteIdByName("IdleRightDown" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[6].name = "IdleRightUp";

            spriteAnimation.clips[6].wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[6].frames.Length; i++)
                {
                    spriteAnimation.clips[6].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[6].frames[frameNumber].spriteId = sc.GetSpriteIdByName("IdleRightUp" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[7].name = "IdleUp";

            spriteAnimation.clips[7].wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[7].frames.Length; i++)
                {
                    spriteAnimation.clips[7].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[7].frames[frameNumber].spriteId = sc.GetSpriteIdByName("IdleUp" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[8].name = "MoveDown";

            spriteAnimation.clips[8].wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[8].frames.Length; i++)
                {
                    spriteAnimation.clips[8].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[8].frames[frameNumber].spriteId = sc.GetSpriteIdByName("MoveDown" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[9].name = "MoveLeft";

            spriteAnimation.clips[9].wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[9].frames.Length; i++)
                {
                    spriteAnimation.clips[9].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[9].frames[frameNumber].spriteId = sc.GetSpriteIdByName("MoveLeft" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[10].name = "MoveLeftDown";

            spriteAnimation.clips[10].wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[10].frames.Length; i++)
                {
                    spriteAnimation.clips[10].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[10].frames[frameNumber].spriteId = sc.GetSpriteIdByName("MoveLeftDown" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[11].name = "MoveLeftUp";

            spriteAnimation.clips[11].wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[11].frames.Length; i++)
                {
                    spriteAnimation.clips[11].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[11].frames[frameNumber].spriteId = sc.GetSpriteIdByName("MoveLeftUp" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[12].name = "MoveRight";

            spriteAnimation.clips[12].wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[12].frames.Length; i++)
                {
                    spriteAnimation.clips[12].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[12].frames[frameNumber].spriteId = sc.GetSpriteIdByName("MoveRight" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[13].name = "MoveRightDown";

            spriteAnimation.clips[13].wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[13].frames.Length; i++)
                {
                    spriteAnimation.clips[13].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[13].frames[frameNumber].spriteId = sc.GetSpriteIdByName("MoveRightDown" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[14].name = "MoveRightUp";

            spriteAnimation.clips[14].wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[14].frames.Length; i++)
                {
                    spriteAnimation.clips[14].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[14].frames[frameNumber].spriteId = sc.GetSpriteIdByName("MoveRightUp" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[15].name = "MoveUp";

            spriteAnimation.clips[15].wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[15].frames.Length; i++)
                {
                    spriteAnimation.clips[15].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[15].frames[frameNumber].spriteId = sc.GetSpriteIdByName("MoveUp" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            EditorUtility.SetDirty(spriteAnimation);
            AssetDatabase.SaveAssets();
            Debug.Log(name + " Over");
        }
    }

    [MenuItem("Ext/GenerateSoldierSpriteAnimation")]
    public static void GenerateSoldierSpriteAnimation()
    {
        tk2dSpriteAnimation spriteAnimation = Selection.activeGameObject.GetComponent<tk2dSpriteAnimation>();
        string name = spriteAnimation.name.Replace("SpriteAnimation", string.Empty);
        string spriteCollectionName = name + "SpriteCollection";
        tk2dSpriteCollectionData sc = null;

        tk2dSpriteCollectionIndex[] items = tk2dEditorUtility.GetOrCreateIndex().GetSpriteCollectionIndex();
        foreach (tk2dSpriteCollectionIndex item in items)
        {
            if (spriteCollectionName == item.name)
            {
                GameObject scgo = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(item.spriteCollectionDataGUID), typeof(GameObject)) as GameObject;
                sc = scgo.GetComponent<tk2dSpriteCollectionData>();
                break;
            }
        }

        if (spriteAnimation != null)
        {
            Debug.Log(spriteAnimation.clips[0].name);
            spriteAnimation.clips[0].name = "AttackDown";

            spriteAnimation.clips[0].wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[0].frames.Length; i++)
                {
                    spriteAnimation.clips[0].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[0].frames[frameNumber].spriteId = sc.GetSpriteIdByName("AttackDown" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[1].name = "AttackLeft";

            spriteAnimation.clips[1].wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[1].frames.Length; i++)
                {
                    spriteAnimation.clips[1].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[1].frames[frameNumber].spriteId = sc.GetSpriteIdByName("AttackLeft" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[2].name = "AttackLeftDown";

            spriteAnimation.clips[2].wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[2].frames.Length; i++)
                {
                    spriteAnimation.clips[2].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[2].frames[frameNumber].spriteId = sc.GetSpriteIdByName("AttackLeftDown" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[3].name = "AttackLeftUp";

            spriteAnimation.clips[3].wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[3].frames.Length; i++)
                {
                    spriteAnimation.clips[3].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[3].frames[frameNumber].spriteId = sc.GetSpriteIdByName("AttackLeftUp" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[4].name = "AttackRight";

            spriteAnimation.clips[4].wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[4].frames.Length; i++)
                {
                    spriteAnimation.clips[4].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[4].frames[frameNumber].spriteId = sc.GetSpriteIdByName("AttackRight" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[5].name = "AttackRightDown";

            spriteAnimation.clips[5].wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[5].frames.Length; i++)
                {
                    spriteAnimation.clips[5].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[5].frames[frameNumber].spriteId = sc.GetSpriteIdByName("AttackRightDown" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[6].name = "AttackRightUp";

            spriteAnimation.clips[6].wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[6].frames.Length; i++)
                {
                    spriteAnimation.clips[6].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[6].frames[frameNumber].spriteId = sc.GetSpriteIdByName("AttackRightUp" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[7].name = "AttackUp";

            spriteAnimation.clips[7].wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[7].frames.Length; i++)
                {
                    spriteAnimation.clips[7].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[7].frames[frameNumber].spriteId = sc.GetSpriteIdByName("AttackUp" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[8].name = "IdleDown";

            spriteAnimation.clips[8].wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[8].frames.Length; i++)
                {
                    spriteAnimation.clips[8].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[8].frames[frameNumber].spriteId = sc.GetSpriteIdByName("IdleDown" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[9].name = "IdleLeft";

            spriteAnimation.clips[9].wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[9].frames.Length; i++)
                {
                    spriteAnimation.clips[9].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[9].frames[frameNumber].spriteId = sc.GetSpriteIdByName("IdleLeft" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[10].name = "IdleLeftDown";

            spriteAnimation.clips[10].wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[10].frames.Length; i++)
                {
                    spriteAnimation.clips[10].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[10].frames[frameNumber].spriteId = sc.GetSpriteIdByName("IdleLeftDown" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[11].name = "IdleLeftUp";

            spriteAnimation.clips[11].wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[11].frames.Length; i++)
                {
                    spriteAnimation.clips[11].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[11].frames[frameNumber].spriteId = sc.GetSpriteIdByName("IdleLeftUp" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[12].name = "IdleRight";

            spriteAnimation.clips[12].wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[12].frames.Length; i++)
                {
                    spriteAnimation.clips[12].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[12].frames[frameNumber].spriteId = sc.GetSpriteIdByName("IdleRight" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[13].name = "IdleRightDown";

            spriteAnimation.clips[13].wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[13].frames.Length; i++)
                {
                    spriteAnimation.clips[13].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[13].frames[frameNumber].spriteId = sc.GetSpriteIdByName("IdleRightDown" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[14].name = "IdleRightUp";

            spriteAnimation.clips[14].wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[14].frames.Length; i++)
                {
                    spriteAnimation.clips[14].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[14].frames[frameNumber].spriteId = sc.GetSpriteIdByName("IdleRightUp" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[15].name = "IdleUp";

            spriteAnimation.clips[15].wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[15].frames.Length; i++)
                {
                    spriteAnimation.clips[15].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[15].frames[frameNumber].spriteId = sc.GetSpriteIdByName("IdleUp" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[16].name = "MoveDown";

            spriteAnimation.clips[16].wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[16].frames.Length; i++)
                {
                    spriteAnimation.clips[16].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[16].frames[frameNumber].spriteId = sc.GetSpriteIdByName("MoveDown" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[17].name = "MoveLeft";

            spriteAnimation.clips[17].wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[17].frames.Length; i++)
                {
                    spriteAnimation.clips[17].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[17].frames[frameNumber].spriteId = sc.GetSpriteIdByName("MoveLeft" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[18].name = "MoveLeftDown";

            spriteAnimation.clips[18].wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[18].frames.Length; i++)
                {
                    spriteAnimation.clips[18].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[18].frames[frameNumber].spriteId = sc.GetSpriteIdByName("MoveLeftDown" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[19].name = "MoveLeftUp";

            spriteAnimation.clips[19].wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[19].frames.Length; i++)
                {
                    spriteAnimation.clips[19].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[19].frames[frameNumber].spriteId = sc.GetSpriteIdByName("MoveLeftUp" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[20].name = "MoveRight";

            spriteAnimation.clips[20].wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[20].frames.Length; i++)
                {
                    spriteAnimation.clips[20].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[20].frames[frameNumber].spriteId = sc.GetSpriteIdByName("MoveRight" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[21].name = "MoveRightDown";

            spriteAnimation.clips[21].wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[21].frames.Length; i++)
                {
                    spriteAnimation.clips[21].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[21].frames[frameNumber].spriteId = sc.GetSpriteIdByName("MoveRightDown" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[22].name = "MoveRightUp";

            spriteAnimation.clips[22].wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[22].frames.Length; i++)
                {
                    spriteAnimation.clips[22].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[22].frames[frameNumber].spriteId = sc.GetSpriteIdByName("MoveRightUp" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[23].name = "MoveUp";

            spriteAnimation.clips[23].wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[23].frames.Length; i++)
                {
                    spriteAnimation.clips[23].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[23].frames[frameNumber].spriteId = sc.GetSpriteIdByName("MoveUp" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            EditorUtility.SetDirty(spriteAnimation);
            AssetDatabase.SaveAssets();
            Debug.Log(name + " Over");
        }
    }

    [MenuItem("Ext/GenerateDefenseTowerSpriteAnimation")]
    public static void GenerateDefenseTowerSpriteAnimation()
    {	
        tk2dSpriteAnimation spriteAnimation = Selection.activeGameObject.GetComponent<tk2dSpriteAnimation>();
        string name = spriteAnimation.name.Replace("SpriteAnimation", string.Empty);
        string spriteCollectionName = name + "SpriteCollection";
        tk2dSpriteCollectionData sc = null;

        tk2dSpriteCollectionIndex[] items = tk2dEditorUtility.GetOrCreateIndex().GetSpriteCollectionIndex();
        foreach (tk2dSpriteCollectionIndex item in items)
        {
            if (spriteCollectionName == item.name)
            {
                GameObject scgo = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(item.spriteCollectionDataGUID), typeof(GameObject)) as GameObject;
                sc = scgo.GetComponent<tk2dSpriteCollectionData>();
                break;
            }
        }

        if (spriteAnimation != null)
        {
            Debug.Log(spriteAnimation.clips[0].name);
            spriteAnimation.clips[0].name = "AttackDown";

            spriteAnimation.clips[0].wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[0].frames.Length; i++)
                {
                    spriteAnimation.clips[0].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[0].frames[frameNumber].spriteId = sc.GetSpriteIdByName("AttackDown" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[1].name = "AttackLeft";

            spriteAnimation.clips[1].wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[1].frames.Length; i++)
                {
                    spriteAnimation.clips[1].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[1].frames[frameNumber].spriteId = sc.GetSpriteIdByName("AttackLeft" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[2].name = "AttackLeftDown";

            spriteAnimation.clips[2].wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[2].frames.Length; i++)
                {
                    spriteAnimation.clips[2].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[2].frames[frameNumber].spriteId = sc.GetSpriteIdByName("AttackLeftDown" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[3].name = "AttackLeftUp";

            spriteAnimation.clips[3].wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[3].frames.Length; i++)
                {
                    spriteAnimation.clips[3].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[3].frames[frameNumber].spriteId = sc.GetSpriteIdByName("AttackLeftUp" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[4].name = "AttackRight";

            spriteAnimation.clips[4].wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[4].frames.Length; i++)
                {
                    spriteAnimation.clips[4].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[4].frames[frameNumber].spriteId = sc.GetSpriteIdByName("AttackRight" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[5].name = "AttackRightDown";

            spriteAnimation.clips[5].wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[5].frames.Length; i++)
                {
                    spriteAnimation.clips[5].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[5].frames[frameNumber].spriteId = sc.GetSpriteIdByName("AttackRightDown" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[6].name = "AttackRightUp";

            spriteAnimation.clips[6].wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[6].frames.Length; i++)
                {
                    spriteAnimation.clips[6].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[6].frames[frameNumber].spriteId = sc.GetSpriteIdByName("AttackRightUp" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[7].name = "AttackUp";

            spriteAnimation.clips[7].wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[7].frames.Length; i++)
                {
                    spriteAnimation.clips[7].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[7].frames[frameNumber].spriteId = sc.GetSpriteIdByName("AttackUp" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[8].name = "IdleDown";

            spriteAnimation.clips[8].wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[8].frames.Length; i++)
                {
                    spriteAnimation.clips[8].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[8].frames[frameNumber].spriteId = sc.GetSpriteIdByName("IdleDown" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[9].name = "IdleLeft";

            spriteAnimation.clips[9].wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[9].frames.Length; i++)
                {
                    spriteAnimation.clips[9].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[9].frames[frameNumber].spriteId = sc.GetSpriteIdByName("IdleLeft" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[10].name = "IdleLeftDown";

            spriteAnimation.clips[10].wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[10].frames.Length; i++)
                {
                    spriteAnimation.clips[10].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[10].frames[frameNumber].spriteId = sc.GetSpriteIdByName("IdleLeftDown" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[11].name = "IdleLeftUp";

            spriteAnimation.clips[11].wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[11].frames.Length; i++)
                {
                    spriteAnimation.clips[11].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[11].frames[frameNumber].spriteId = sc.GetSpriteIdByName("IdleLeftUp" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[12].name = "IdleRight";

            spriteAnimation.clips[12].wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[12].frames.Length; i++)
                {
                    spriteAnimation.clips[12].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[12].frames[frameNumber].spriteId = sc.GetSpriteIdByName("IdleRight" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[13].name = "IdleRightDown";

            spriteAnimation.clips[13].wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[13].frames.Length; i++)
                {
                    spriteAnimation.clips[13].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[13].frames[frameNumber].spriteId = sc.GetSpriteIdByName("IdleRightDown" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[14].name = "IdleRightUp";

            spriteAnimation.clips[14].wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[14].frames.Length; i++)
                {
                    spriteAnimation.clips[14].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[14].frames[frameNumber].spriteId = sc.GetSpriteIdByName("IdleRightUp" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[15].name = "IdleUp";

            spriteAnimation.clips[15].wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[15].frames.Length; i++)
                {
                    spriteAnimation.clips[15].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[15].frames[frameNumber].spriteId = sc.GetSpriteIdByName("IdleUp" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            EditorUtility.SetDirty(spriteAnimation);
            AssetDatabase.SaveAssets();
            Debug.Log(name + " Over");
        }
    }

    [MenuItem("Ext/GenerateFortBerserkerSpriteAnimation")]
    public static void GenerateFortBerserkerSpriteAnimation()
    {
        tk2dSpriteAnimation spriteAnimation = Selection.activeGameObject.GetComponent<tk2dSpriteAnimation>();
        string name = spriteAnimation.name.Replace("SpriteAnimation", string.Empty);
        string spriteCollectionName = name + "SpriteCollection";
        tk2dSpriteCollectionData sc = null;

        tk2dSpriteCollectionIndex[] items = tk2dEditorUtility.GetOrCreateIndex().GetSpriteCollectionIndex();
        foreach (tk2dSpriteCollectionIndex item in items)
        {
            if (spriteCollectionName == item.name)
            {
                GameObject scgo = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(item.spriteCollectionDataGUID), typeof(GameObject)) as GameObject;
                sc = scgo.GetComponent<tk2dSpriteCollectionData>();
                break;
            }
        }

        if (spriteAnimation != null)
        {
            Debug.Log(spriteAnimation.clips[0].name);
            spriteAnimation.clips[0].name = "AttackDown";

            spriteAnimation.clips[0].wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[0].frames.Length; i++)
                {
                    spriteAnimation.clips[0].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[0].frames[frameNumber].spriteId = sc.GetSpriteIdByName("AttackDown" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[1].name = "AttackLeft";

            spriteAnimation.clips[1].wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[1].frames.Length; i++)
                {
                    spriteAnimation.clips[1].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[1].frames[frameNumber].spriteId = sc.GetSpriteIdByName("AttackLeft" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[2].name = "AttackLeftDown";

            spriteAnimation.clips[2].wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[2].frames.Length; i++)
                {
                    spriteAnimation.clips[2].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[2].frames[frameNumber].spriteId = sc.GetSpriteIdByName("AttackLeftDown" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[3].name = "AttackLeftUp";

            spriteAnimation.clips[3].wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[3].frames.Length; i++)
                {
                    spriteAnimation.clips[3].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[3].frames[frameNumber].spriteId = sc.GetSpriteIdByName("AttackLeftUp" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[4].name = "AttackRight";

            spriteAnimation.clips[4].wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[4].frames.Length; i++)
                {
                    spriteAnimation.clips[4].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[4].frames[frameNumber].spriteId = sc.GetSpriteIdByName("AttackRight" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[5].name = "AttackRightDown";

            spriteAnimation.clips[5].wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[5].frames.Length; i++)
                {
                    spriteAnimation.clips[5].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[5].frames[frameNumber].spriteId = sc.GetSpriteIdByName("AttackRightDown" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[6].name = "AttackRightUp";

            spriteAnimation.clips[6].wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[6].frames.Length; i++)
                {
                    spriteAnimation.clips[6].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[6].frames[frameNumber].spriteId = sc.GetSpriteIdByName("AttackRightUp" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[7].name = "AttackUp";

            spriteAnimation.clips[7].wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[7].frames.Length; i++)
                {
                    spriteAnimation.clips[7].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[7].frames[frameNumber].spriteId = sc.GetSpriteIdByName("AttackUp" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[8].name = "IdleDown";

            spriteAnimation.clips[8].wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[8].frames.Length; i++)
                {
                    spriteAnimation.clips[8].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[8].frames[frameNumber].spriteId = sc.GetSpriteIdByName("IdleDown" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[9].name = "IdleLeft";

            spriteAnimation.clips[9].wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[9].frames.Length; i++)
                {
                    spriteAnimation.clips[9].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[9].frames[frameNumber].spriteId = sc.GetSpriteIdByName("IdleLeft" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[10].name = "IdleLeftDown";

            spriteAnimation.clips[10].wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[10].frames.Length; i++)
                {
                    spriteAnimation.clips[10].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[10].frames[frameNumber].spriteId = sc.GetSpriteIdByName("IdleLeftDown" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[11].name = "IdleLeftUp";

            spriteAnimation.clips[11].wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[11].frames.Length; i++)
                {
                    spriteAnimation.clips[11].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[11].frames[frameNumber].spriteId = sc.GetSpriteIdByName("IdleLeftUp" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[12].name = "IdleRight";

            spriteAnimation.clips[12].wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[12].frames.Length; i++)
                {
                    spriteAnimation.clips[12].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[12].frames[frameNumber].spriteId = sc.GetSpriteIdByName("IdleRight" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[13].name = "IdleRightDown";

            spriteAnimation.clips[13].wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[13].frames.Length; i++)
                {
                    spriteAnimation.clips[13].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[13].frames[frameNumber].spriteId = sc.GetSpriteIdByName("IdleRightDown" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[14].name = "IdleRightUp";

            spriteAnimation.clips[14].wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[14].frames.Length; i++)
                {
                    spriteAnimation.clips[14].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[14].frames[frameNumber].spriteId = sc.GetSpriteIdByName("IdleRightUp" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            spriteAnimation.clips[15].name = "IdleUp";

            spriteAnimation.clips[15].wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            if (sc != null)
            {
                int frameNumber = 0;
                for (int i = 0; i < spriteAnimation.clips[15].frames.Length; i++)
                {
                    spriteAnimation.clips[15].frames[frameNumber].spriteCollection = sc;
                    spriteAnimation.clips[15].frames[frameNumber].spriteId = sc.GetSpriteIdByName("IdleUp" + (frameNumber + 1).ToString("00"));
                    frameNumber++;
                }
            }

            EditorUtility.SetDirty(spriteAnimation);
            AssetDatabase.SaveAssets();
            Debug.Log(name + " Over");
        }
    }
}
