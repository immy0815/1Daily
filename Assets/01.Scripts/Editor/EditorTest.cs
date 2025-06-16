using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class EditorTest
{
  [Test]
  public void EditorTestSimplePasses()
  {
    // Assert 클래스를 사용하여 조건을 테스트할 수 있습니다.
    Debug.Log("EditorTestSimplePasses");
  }

  // UnityTest는 PlayMode에서 코루틴처럼 동작합니다.
  [UnityTest]
  public IEnumerator EditorTestWithEnumeratorPasses()
  { 
    // Assert 클래스를 사용하여 조건을 테스트할 수 있습니다.
    // 프레임을 건너뛰기 위해 yield return null를 사용하면 됩니다.
    Debug.Log("EditorTestWithEnumeratorPasses");
    yield return null;
  }
}