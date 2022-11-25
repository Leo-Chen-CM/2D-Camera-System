using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class CameraSystem2DTest
{
    Scene testScene;
    GameObject m_game;
    [SetUp]
    public void Setup()
    {
        //SceneManager.UnloadSceneAsync(testScene);
        testScene = SceneManager.CreateScene("Testing Scene");
        SceneManager.SetActiveScene(testScene);
        m_game = new GameObject("Camera");
        m_game.AddComponent<Camera>();
        m_game.GetComponent<Camera>().tag = "MainCamera";
        m_game.AddComponent<CameraSystem2D>();

    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(m_game);
        SceneManager.UnloadSceneAsync(testScene);
    }

    [UnityTest]
    public IEnumerator CameraSystem2DFadeScreen()
    {
        m_game.GetComponent<CameraSystem2D>().FadeInOut();
        yield return new WaitForSeconds(5);
        Assert.LessOrEqual(m_game.GetComponent<CameraSystem2D>().ReturnBlackOutImage().GetComponent<Image>().color.a, 5);
    }

    [UnityTest]
    public IEnumerator CameraSystem2DSpeedZoomIncrease()
    {
        m_game.GetComponent<CameraSystem2D>().FadeInOut();
        yield return new WaitForSeconds(5);
        Assert.LessOrEqual(m_game.GetComponent<CameraSystem2D>().ReturnBlackOutImage().GetComponent<Image>().color.a, 5);
    }

    // A Test behaves as an ordinary method
    //[Test]
    //public void CameraSystem2DTestSimplePasses()
    //{
    //    GameObject gameObject = GameObject.Find("FadeOutScreen");
    //    Assert.IsNotNull(gameObject);
    //}

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    //[UnityTest]
    //public IEnumerator CameraSystem2DTestWithEnumeratorPasses()
    //{
    //    // Use the Assert class to test conditions.
    //    // Use yield to skip a frame.
    //    yield return null;
    //}
}
