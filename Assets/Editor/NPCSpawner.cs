using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class NPCSpawner : EditorWindow
{
    string objectBaseName = "";
    string gender = "";
    int genderInt;
    GameObject objectToSpawn;
    Vector3 spawnPos;

    public List<GameObject> hairFem, hairMale, ears, eyebrows, eyelashes, topFem, topMale, bottomFem, bottomMale, shoeFem, shoeMale = new List<GameObject>();
    public List<Material> skins, eyes = new List<Material>();

    MaterialList listHolder;

    GameObject hair;
    GameObject ear;
    GameObject eyebrow;
    GameObject eyelash;
    GameObject top;
    GameObject bottom;
    GameObject shoe;

    [MenuItem("Tools/NPC Spawner and Randomizer")]
    public static void ShowWindow()
    {
        GetWindow(typeof(NPCSpawner));      //GetWindow is a method inherited from the EditorWindow class
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(0, 0, 700, 500));
        GUILayout.Label("Spawn New NPC", EditorStyles.boldLabel);
        GUILayout.Label("Requires an object with blendshapes & accessories correctly tagged and named.");

        GUILayout.BeginHorizontal();
        spawnPos.x = EditorGUILayout.IntField("X", 0);
        spawnPos.y = EditorGUILayout.IntField("Y", 0);
        spawnPos.z = EditorGUILayout.IntField("Z", 0);
        GUILayout.EndHorizontal();
        
        objectToSpawn = EditorGUILayout.ObjectField("Prefab to Spawn", objectToSpawn, typeof(GameObject), false) as GameObject;
        listHolder = EditorGUILayout.ObjectField("Object with Materials List", listHolder, typeof(MaterialList), false) as MaterialList;

        GUILayout.Box(AssetPreview.GetAssetPreview(objectToSpawn), GUILayout.MaxHeight(200), GUILayout.ExpandWidth(true));

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Randomize NPC"))
        {
            Randomize();
            objectBaseName = EditorGUILayout.TextField("NPC Name", objectBaseName);
            GUILayout.Label("Gender: " + gender);
            GUILayout.Label("Hairstyle: " + hair.name);
            GUILayout.Label("Ears: " + ear.name);
            GUILayout.Label("Eyebrows: " + eyebrow.name);
            GUILayout.Label("Eyelashes: " + eyelash.name);
            GUILayout.Label("Clothing Top: " + top.name);
            GUILayout.Label("Clothing Bottom: " + bottom.name);
            GUILayout.Label("Clothing Shoe: " + shoe.name);
            //Shapekey sliders
        }

        if (GUILayout.Button("Spawn NPC"))
        {
            SpawnObject();
        }

        GUILayout.EndArea();
    }
    
    /// <summary>
    /// Randomizes everything needed for a character.
    /// </summary>
    private void Randomize()
    {
        int roll = Random.Range(1, 2);
        genderInt = roll;
        AssignExtraItems();
        RandomizeGear();
        RandomizeShapeKeys();
        RandomizeSkinAndEyes();
    }

    #region Randomization

    /// <summary>
    /// Randomizes skincolor and eyecolor.
    /// </summary>
    private void RandomizeSkinAndEyes()
    {
        //Skin color randomization
        int rand = Random.Range(0, listHolder.skinMaterials.Count);
        Material mat = listHolder.skinMaterials.ElementAt(rand);
        objectToSpawn.transform.Find("Body").GetComponent<Renderer>().material = mat;

        // Eye color randomization
        rand = Random.Range(0, listHolder.eyeMaterials.Count);
        mat = listHolder.eyeMaterials.ElementAt(rand);
        objectToSpawn.transform.Find("Eyes").GetComponent<Renderer>().material = mat;
    }

    /// <summary>
    /// Randomizes targets shapekeys to create unique face.
    /// </summary>
    private void RandomizeShapeKeys()
    {
        //shapekey randomization
    }

    /// <summary>
    /// Randomizes assigned child gameobjects
    /// </summary>
    private void RandomizeGear()
    {
        int rand;
        GameObject selection;
        if (genderInt == 1) //Female Items
        {
            rand = Random.Range(0, hairFem.Count);
            selection = hairFem.ElementAt(rand);
            selection.SetActive(true);
        }
        if (genderInt == 2) //Male Items
        {
            rand = Random.Range(0, hairMale.Count);
            selection = hairMale.ElementAt(rand);
            selection.SetActive(true);
        }

        //General Items
        rand = Random.Range(0, ears.Count);
        selection = ears.ElementAt(rand);
        selection.SetActive(true);

        rand = Random.Range(0, eyebrows.Count);
        selection = eyebrows.ElementAt(rand);
        selection.SetActive(true);

        rand = Random.Range(0, eyelashes.Count);
        selection = eyelashes.ElementAt(rand);
        selection.SetActive(true);

    }

    #endregion

    /// <summary>
    /// Assigns gameobjects children to correct lists based on tag and gender
    /// </summary>
    /// <param name="rolledGender">Gender roll. 1 or 2. Int</param>
    private void AssignExtraItems()
    {
        foreach (Transform child in objectToSpawn.transform)
        {
            child.gameObject.SetActive(false);

            if (genderInt == 1) // Only add Female items to list if we are going to use them.
            {
                gender = "Female";
                if (child.CompareTag("HairFem"))
                {
                    hairFem.Add(child.gameObject);
                }
                if (child.CompareTag("TopFem"))
                {
                    topFem.Add(child.gameObject);
                }
                if (child.CompareTag("BottomFem"))
                {
                    bottomFem.Add(child.gameObject);
                }
                if (child.CompareTag("ShoeFem"))
                {
                    shoeFem.Add(child.gameObject);
                }
            }
            else if (genderInt == 2)// Only add Male items to list if we are going to use them.
            {
                gender = "Male";
                if (child.CompareTag("HairMale"))
                {
                    hairMale.Add(child.gameObject);
                }
                if (child.CompareTag("TopMale"))
                {
                    topMale.Add(child.gameObject);
                }
                if (child.CompareTag("BottomMale"))
                {
                    bottomMale.Add(child.gameObject);
                }
                if (child.CompareTag("ShoeMale"))
                {
                    shoeMale.Add(child.gameObject);
                }
            }

            // Universal Items
            if (child.CompareTag("Ears"))
            {
                ears.Add(child.gameObject);
            }
            if (child.CompareTag("Brows"))
            {
                eyebrows.Add(child.gameObject);
            }
            if (child.CompareTag("Lashes"))
            {
                eyelashes.Add(child.gameObject);
            }
        }
    }

    private void SpawnObject()
    {
        if (objectToSpawn == null)
        {
            Debug.LogError("Error: Please assign an object to be spawned.");
            return;
        }
        if (objectBaseName == string.Empty)
        {
            Debug.LogError("Error: Please enter a base name for the object.");
            return;
        }


        GameObject newObject = Instantiate(objectToSpawn, spawnPos, Quaternion.identity);
        newObject.name = objectBaseName;
    }
}