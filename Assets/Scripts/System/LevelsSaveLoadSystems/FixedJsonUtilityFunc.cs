using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FixedJsonUtilityFunc : MonoBehaviour
{

    public static string ToJsonFix(Transform transformObject)
    {
        var jsonT = new JsonTransform();

        jsonT.position = transformObject.position;
        jsonT.rotation = transformObject.rotation;
        jsonT.scale = transformObject.localScale;

        return JsonUtility.ToJson(jsonT);
    }
    
    public static void FromJsonOverwriteFix(string jsonData,Transform transformObject)
    {
        var jsonTransform = new JsonTransform();
        JsonUtility.FromJsonOverwrite(jsonData,jsonTransform);

        transformObject.position = jsonTransform.position;
        transformObject.rotation = jsonTransform.rotation;
        transformObject.localScale = jsonTransform.scale;
    }
    
    
    public static string ToJsonFix(Rigidbody rigidbodyObject)
    {
        var jsonRigidbody = new JsonRigidbody();

        jsonRigidbody.mass = rigidbodyObject.mass;
        jsonRigidbody.drag = rigidbodyObject.drag;
        jsonRigidbody.angularDrag = rigidbodyObject.angularDrag;
        jsonRigidbody.useGravity = rigidbodyObject.useGravity;
        jsonRigidbody.isKinematic = rigidbodyObject.isKinematic;
        jsonRigidbody.interpolation = rigidbodyObject.interpolation;
        jsonRigidbody.collisionDetectionMode = rigidbodyObject.collisionDetectionMode;
        jsonRigidbody.constraints = rigidbodyObject.constraints;

        jsonRigidbody.velocity = rigidbodyObject.velocity;
        jsonRigidbody.angularVelocity = rigidbodyObject.angularVelocity;
        
        return JsonUtility.ToJson(jsonRigidbody);
    }
    
    public static void FromJsonOverwriteFix(string jsonData,Rigidbody rigidbodyObject)
    {
        var jsonRigidbody = new JsonRigidbody();
        JsonUtility.FromJsonOverwrite(jsonData,jsonRigidbody);

        rigidbodyObject.mass = jsonRigidbody.mass;
        rigidbodyObject.drag = jsonRigidbody.drag;
        rigidbodyObject.angularDrag = jsonRigidbody.angularDrag;
        rigidbodyObject.useGravity = jsonRigidbody.useGravity;
        rigidbodyObject.isKinematic = jsonRigidbody.isKinematic;
        rigidbodyObject.interpolation = jsonRigidbody.interpolation;
        rigidbodyObject.collisionDetectionMode = jsonRigidbody.collisionDetectionMode;
        rigidbodyObject.constraints = jsonRigidbody.constraints;

        rigidbodyObject.velocity = jsonRigidbody.velocity;
        rigidbodyObject.angularVelocity = jsonRigidbody.angularVelocity;

    }
    
    
    
    public static string ToJsonFix<TKey,TValue>(Dictionary<TKey,TValue> dictionary)
    {
        var jsonDictionary = GetJsonVersion(dictionary);

        return JsonUtility.ToJson(jsonDictionary);
    }
    
    public static void FromJsonOverwriteFix<TKey,TValue>(string jsonData, Dictionary<TKey,TValue> dictionary)
    {
        var jsonDictionary = JsonUtility.FromJson<JsonDictionary<TKey, TValue>>(jsonData);
        
        JsonToNormal(jsonDictionary,dictionary);
    }
    
    
    public static JsonTransform GetJsonVersion(Transform t)
    {
        return new JsonTransform(t.position,t.rotation,t.localScale);
    }
    
    public static JsonRigidbody GetJsonVersion(Rigidbody rb)
    {
        return new JsonRigidbody(
            rb.mass,
            rb.drag,
            rb.angularDrag,
            rb.useGravity,
            rb.isKinematic,
            rb.interpolation,
            rb.collisionDetectionMode,rb.constraints,rb.velocity,rb.angularVelocity);
    }
    
    public static JsonBoxCollider GetJsonVersion(BoxCollider boxCollider)
    {
        return new JsonBoxCollider(
            boxCollider.isTrigger,
            boxCollider.material,
            boxCollider.center,
            boxCollider.size);
    }

    public static JsonDictionary<TKey,TValue> GetJsonVersion<TKey,TValue>(Dictionary<TKey,TValue> dictionary)
    {
        var jsonDictionary = new JsonDictionary<TKey, TValue>(dictionary);

        return jsonDictionary;
    }


    public static void JsonToNormal(JsonTransform jsonT, Transform normalT)
    {
        normalT.position = jsonT.position;
        normalT.rotation = jsonT.rotation;
        normalT.localScale = jsonT.scale;
    }
    
    public static void JsonToNormal(JsonRigidbody jsonRb, Rigidbody normalRb)
    {
        normalRb.mass = jsonRb.mass;
        normalRb.drag = jsonRb.drag;
        normalRb.angularDrag = jsonRb.angularDrag;
        normalRb.useGravity = jsonRb.useGravity;
        normalRb.isKinematic = jsonRb.isKinematic;
        normalRb.interpolation = jsonRb.interpolation;
        normalRb.collisionDetectionMode = jsonRb.collisionDetectionMode;
        normalRb.constraints = jsonRb.constraints;
        normalRb.velocity = jsonRb.velocity;
        normalRb.angularVelocity = jsonRb.velocity;
    }
    
    public static void JsonToNormal(JsonBoxCollider jsonBoxCollider, BoxCollider normalRbBoxCollider)
    {
        normalRbBoxCollider.isTrigger = jsonBoxCollider.isTrigger;
        normalRbBoxCollider.center = jsonBoxCollider.center;
        normalRbBoxCollider.size = jsonBoxCollider.size;

        normalRbBoxCollider.material = jsonBoxCollider.physMat;
    }
    
    public static void JsonToNormal<TKey,TValue> (JsonDictionary<TKey,TValue> jsonDictionary, Dictionary<TKey,TValue> normalDictionary)
    {
        normalDictionary.Clear();
        
        for (int i = 0; i < jsonDictionary.keys.Count; i++)
        {
            var jsonKey = jsonDictionary.keys[i];
            var jsonValue = jsonDictionary.values[i];
            
            normalDictionary.Add(jsonKey,jsonValue);
        }
        
    }
    
}

[Serializable]
public class JsonTransform
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
    
    public JsonTransform(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        this.position = position;
        this.rotation = rotation;
        this.scale = scale;
    }
    
    public JsonTransform()
    {
    }
}

[Serializable]
public class JsonRigidbody
{
    public float mass;
    public float drag;
    public float angularDrag;
    public bool useGravity;
    public bool isKinematic;
    public RigidbodyInterpolation interpolation;
    public CollisionDetectionMode collisionDetectionMode;
    public RigidbodyConstraints constraints;

    public Vector3 velocity;
    public Vector3 angularVelocity;

    public JsonRigidbody(float mass, float drag, float angularDrag, bool useGravity, bool isKinematic, RigidbodyInterpolation interpolation, CollisionDetectionMode collisionDetectionMode, RigidbodyConstraints constraints, Vector3 velocity, Vector3 angularVelocity)
    {
        this.mass = mass;
        this.drag = drag;
        this.angularDrag = angularDrag;
        this.useGravity = useGravity;
        this.isKinematic = isKinematic;
        this.interpolation = interpolation;
        this.collisionDetectionMode = collisionDetectionMode;
        this.constraints = constraints;
        this.velocity = velocity;
        this.angularVelocity = angularVelocity;
    }
    
    public JsonRigidbody()
    {
    }
    
}

[Serializable]
public class JsonBoxCollider
{
    public bool isTrigger;
    public PhysicMaterial physMat;
    public Vector3 center;
    public Vector3 size;

    public JsonBoxCollider(bool isTrigger, PhysicMaterial physMat, Vector3 center, Vector3 size)
    {
        this.isTrigger = isTrigger;
        this.physMat = physMat;
        this.center = center;
        this.size = size;
    }
}

[Serializable]
public class JsonDictionary<TKey,TValue>
{
    public List<TKey> keys = new List<TKey>();
    public List<TValue> values = new List<TValue>();

    public JsonDictionary(Dictionary<TKey,TValue> dictionary)
    {

        foreach (var key in dictionary.Keys)
        {
            keys.Add(key);
        }

        foreach (var value in dictionary.Values)
        {
            values.Add(value);
        }
        
    }
}