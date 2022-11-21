using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class CameraSystem2DTest
{
    Scene testScene;
    Camera m_mainCamera;
    GameObject m_game;
    [SetUp]
    public void Setup()
    {
        testScene = SceneManager.CreateScene("Testing Scene");
        SceneManager.SetActiveScene(testScene);
        m_mainCamera = new Camera();
        m_mainCamera.tag = "MainCamera";
        m_game = new GameObject("Camera",typeof(CameraSystem2D));
        //m_gameObject.AddComponent<Camera>();

    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(m_mainCamera);
    }

    [UnityTest]
    public IEnumerator CameraSystem2DFadeScreen()
    {
        yield return new WaitForSeconds(10);
    }

    // A Test behaves as an ordinary method
    [Test]
    public void CameraSystem2DTestSimplePasses()
    {
        GameObject gameObject = GameObject.Find("FadeOutScreen");
        Assert.IsNotNull(gameObject);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator CameraSystem2DTestWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
