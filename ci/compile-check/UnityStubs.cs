// Stubs minimos da API do Unity usados apenas para verificacao de compilacao no CI.
// Nao implementam comportamento. Nao usar em runtime.
#pragma warning disable
using System;

namespace UnityEngine
{
    public class Object
    {
        public string name;
        public static void Destroy(Object o, float t = 0) { }
        public static T Instantiate<T>(T o, Vector3 p, Quaternion r) where T : Object => o;
        public static T Instantiate<T>(T o) where T : Object => o;
        public static T FindObjectOfType<T>() where T : Object => null; public static T[] FindObjectsOfType<T>() where T : Object => null; public static T Instantiate<T>(T o, Transform parent) where T : Object => o;
        public static implicit operator bool(Object o) => !ReferenceEquals(o, null);
    }
    public class Component : Object
    {
        public GameObject gameObject; public Transform transform; public string tag;
        public T GetComponent<T>() => default; public T GetComponentInChildren<T>() => default;
        public bool CompareTag(string t) => false;
    }
    public class Behaviour : Component { public bool enabled; }
    public class MonoBehaviour : Behaviour
    {
        public void Invoke(string m, float t) { } public void CancelInvoke() { } public void CancelInvoke(string m) { }
        public bool IsInvoking(string m) => false;
        public Coroutine StartCoroutine(System.Collections.IEnumerator e) => null; public void StopAllCoroutines() { }
    }
    public class Coroutine { }
    public class WaitForSeconds { public WaitForSeconds(float s) { } }
    public class GameObject : Object
    {
        public GameObject() { } public GameObject(string n) { }
        public Transform transform; public bool activeSelf; public string tag;
        public void SetActive(bool v) { } public T GetComponent<T>() => default; public T AddComponent<T>() => default;
        public bool CompareTag(string t) => false; public T GetComponentInChildren<T>() => default;
    }
    public class Transform : Component, System.Collections.IEnumerable
    {
        public System.Collections.IEnumerator GetEnumerator() => null;
        public Vector3 position, localPosition, eulerAngles, localEulerAngles, forward, right, up, localScale;
        public Quaternion rotation, localRotation; public Transform parent;
        public void Translate(Vector3 v) { } public void Rotate(Vector3 v) { } public void Rotate(float x, float y, float z) { }
        public void LookAt(Transform t) { } public void LookAt(Vector3 t) { } public Transform Find(string n) => null; public void SetParent(Transform p) { }
    }
    public class RectTransform : Transform { public Vector2 anchoredPosition, sizeDelta; }
    public struct Vector2 { public float x, y; public Vector2(float x, float y) { this.x = x; this.y = y; } public static Vector2 zero; public static Vector2 operator /(Vector2 a, float b) => a; public static Vector2 operator *(Vector2 a, float b) => a; public static Vector2 operator +(Vector2 a, Vector2 b) => a; public static Vector2 operator -(Vector2 a, Vector2 b) => a; public static implicit operator Vector2(Vector3 v) => default; public static implicit operator Vector3(Vector2 v) => default; }
    public struct Vector3
    {
        public float x, y, z; public Vector3(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }
        public float magnitude => 0; public Vector3 normalized => this;
        public static Vector3 zero, one, up, down, forward, back, right, left;
        public static Vector3 Cross(Vector3 a, Vector3 b) => a; public static float Angle(Vector3 a, Vector3 b) => 0;
        public static float Distance(Vector3 a, Vector3 b) => 0; public static float Dot(Vector3 a, Vector3 b) => 0;
        public static Vector3 Lerp(Vector3 a, Vector3 b, float t) => a;
        public static Vector3 operator +(Vector3 a, Vector3 b) => a; public static Vector3 operator -(Vector3 a, Vector3 b) => a;
        public static Vector3 operator -(Vector3 a) => a; public static Vector3 operator *(Vector3 a, float b) => a;
        public static Vector3 operator *(float b, Vector3 a) => a; public static Vector3 operator /(Vector3 a, float b) => a;
    }
    public struct Quaternion
    {
        public static Quaternion identity; public Vector3 eulerAngles;
        public static Quaternion Euler(float x, float y, float z) => identity; public static Quaternion Euler(Vector3 v) => identity;
        public static Quaternion Lerp(Quaternion a, Quaternion b, float t) => a; public static Quaternion Slerp(Quaternion a, Quaternion b, float t) => a;
    }
    public struct Color
    {
        public float r, g, b, a; public Color(float r, float g, float b, float a = 1) { this.r = r; this.g = g; this.b = b; this.a = a; }
        public static Color yellow, green, red, white, black, clear, gray, blue; public static Color Lerp(Color a, Color b, float t) => a;
    }
    public static class Mathf
    {
        public const float PI = 3.14159f;
        public static float Min(float a, float b) => a; public static int Min(int a, int b) => a;
        public static float Max(float a, float b) => a; public static int Max(int a, int b) => a;
        public static float Clamp(float v, float a, float b) => v; public static int Clamp(int v, int a, int b) => v; public static float Clamp01(float v) => v;
        public static float Round(float v) => v; public static int RoundToInt(float v) => 0; public static int FloorToInt(float v) => 0;
        public static float Lerp(float a, float b, float t) => a; public static float Exp(float v) => v; public static float Abs(float v) => v;
        public static float Sin(float v) => v; public static float Cos(float v) => v; public static float PerlinNoise(float x, float y) => 0;
        public static float MoveTowards(float a, float b, float d) => a;
    }
    public static class Time { public static float deltaTime, time, fixedDeltaTime, timeScale; }
    public static class Debug { public static void Log(object o) { } public static void LogWarning(object o) { } public static void LogError(object o) { } }
    public static class Random { public static float value; public static float Range(float a, float b) => a; public static int Range(int a, int b) => a; }
    public static class Input
    {
        public static float GetAxis(string a) => 0; public static bool GetKeyDown(KeyCode k) => false;
        public static bool GetKey(KeyCode k) => false; public static bool GetKeyUp(KeyCode k) => false;
    }
    public enum KeyCode { Space, R, M, Escape, P, F, W, A, S, D }
    public static class PlayerPrefs
    {
        public static bool HasKey(string k) => false; public static void Save() { } public static void DeleteKey(string k) { }
        public static void SetInt(string k, int v) { } public static int GetInt(string k, int d = 0) => d;
        public static void SetFloat(string k, float v) { } public static float GetFloat(string k, float d = 0) => d;
        public static void SetString(string k, string v) { } public static string GetString(string k, string d = "") => d;
    }
    public static class Application { public static void Quit() { } }
    public static class AudioListener { public static float volume; }
    public static class RenderSettings { public static bool fog; public static float fogDensity; public static Color fogColor; }
    public struct RaycastHit { public float distance; public Vector3 point; public Collider collider; }
    public static class Physics
    {
        public static bool Raycast(Vector3 o, Vector3 d, out RaycastHit h, float m) { h = default; return false; }
        public static Collider[] OverlapSphere(Vector3 p, float r) => new Collider[0]; public static Collider[] OverlapSphere(Vector3 p, float r, int mask) => new Collider[0];
    }
    public class Collider : Component { }
    public struct LayerMask { public int value; public static implicit operator int(LayerMask m) => m.value; }
    public class Camera : Behaviour { public static Camera main; public float fieldOfView; public RenderTexture targetTexture; }
    public class Texture : Object { }
    public class RenderTexture : Texture { }
    public class Collision { public Vector3 relativeVelocity; public GameObject gameObject; public Collider collider; public Transform transform; }
    public enum ForceMode { Force, Impulse, Acceleration, VelocityChange }
    public class Rigidbody : Component
    {
        public Vector3 velocity, angularVelocity; public float mass, drag, angularDrag; public bool useGravity, isKinematic;
        public void AddForce(Vector3 f) { } public void AddForce(Vector3 f, ForceMode m) { }
        public void AddTorque(Vector3 t) { } public void AddTorque(Vector3 t, ForceMode m) { }
    }
    public class AudioClip : Object { }
    public class AudioSource : Behaviour
    {
        public float pitch, volume; public bool isPlaying, loop; public AudioClip clip;
        public void Play() { } public void Stop() { } public void Pause() { } public void PlayOneShot(AudioClip c) { }
    }
    public class Light : Behaviour { public float intensity, range; public Color color; }
    public class Material : Object { public Color color; public void SetColor(string n, Color c) { } public void SetFloat(string n, float f) { } }
    public static class Gizmos { public static Color color; public static void DrawWireSphere(Vector3 c, float r) { } public static void DrawSphere(Vector3 c, float r) { } public static void DrawWireCube(Vector3 c, Vector3 s) { } public static void DrawLine(Vector3 a, Vector3 b) { } }
    public class Renderer : Component { public Material material; public bool enabled; }
    public class AnimationCurve { public float Evaluate(float t) => 0; }
    public class ParticleSystem : Component
    {
        public bool isPlaying, isEmitting; public void Play() { } public void Stop() { } public void Emit(int n) { }
        public MainModule main; public EmissionModule emission;
        public struct MainModule { public float startSpeed, startSize; public MinMaxCurve startLifetime; public Color startColor; }
        public struct EmissionModule { public bool enabled; public MinMaxCurve rateOverTime; }
        public struct MinMaxCurve { public static implicit operator MinMaxCurve(float f) => default; }
    }
    public class HeaderAttribute : Attribute { public HeaderAttribute(string h) { } }
    public class TooltipAttribute : Attribute { public TooltipAttribute(string h) { } }
    public class RangeAttribute : Attribute { public RangeAttribute(float a, float b) { } }
    public class HideInInspector : Attribute { }
    public class SerializeField : Attribute { }
}
namespace UnityEngine.Events { public class UnityEvent { public void AddListener(System.Action a) { } } }
namespace UnityEngine.UI
{
    public class Graphic : Behaviour { public Color color; }
    public class Text : Graphic { public string text; }
    public class RawImage : Graphic { public Texture texture; }
    public class Image : Graphic { public float fillAmount; }
    public class Slider : Behaviour { public float value, maxValue, minValue; }
    public class Toggle : Behaviour { public bool isOn; }
    public class Button : Behaviour { public UnityEngine.Events.UnityEvent onClick; }
}
namespace UnityEngine.SceneManagement { public static class SceneManager { public static void LoadScene(string n) { } public static void LoadScene(int i) { } } }
namespace UnityEngine.Rendering.PostProcessing
{
    public class PostProcessVolume : Behaviour { public PostProcessProfile profile; public float weight; }
    public class PostProcessProfile : Object { public bool TryGetSettings<T>(out T s) { s = default; return false; } }
    public class FloatParameter { public float value; }
    public class ColorParameter { public Color value; }
    public class Vignette { public FloatParameter intensity; public ColorParameter color; }
    public class ChromaticAberration { public FloatParameter intensity; }
    public class ColorGrading { public FloatParameter saturation; }
}
