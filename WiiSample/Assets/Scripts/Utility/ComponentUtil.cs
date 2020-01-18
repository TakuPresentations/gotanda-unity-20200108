using UnityEngine;

using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace Catapult
{
    public class ComponentUtil
    {
        /// <summary>
        /// <para>初期化</para>
        /// <para>【第1引数】初期化したいTransform</para>
        /// </summary>
        public static void Normalize(Transform t)
        {
            t.localPosition = Vector3.zero;
            t.localEulerAngles = Vector3.zero;
            t.localScale = Vector3.one;
        }

        /// <summary>
        /// <para>GameObjectを生成</para>
        /// <para>【第1引数】親となるGameObject</para>
        /// <para>【第2引数】生成したいGameObject(外部から持ってきた物)</para>
        /// <para>【戻り値】生成されたGameObject</para>
        /// </summary>
        public static GameObject InstantiateTo(GameObject parent, GameObject go)
        {
            GameObject ins = (GameObject)GameObject.Instantiate(
                go,
                parent.transform.position,
                parent.transform.rotation
            );
            ins.transform.parent = parent.transform;
            Normalize(ins.transform);
            return ins;
        }

        /// <summary>
        /// <para>GameObjectを生成し、そのGameObjectに張り付いている、Componentクラスを取得する</para>
        /// <para>【第1引数】親となるGameObject</para>
        /// <para>【第2引数】生成したいGameObject(外部から持ってきた物)</para>
        /// <para>【戻り値】生成されたGameObjectに張り付いてる指定したComponentクラス</para>
        /// </summary>
        public static T InstantiateTo<T>(GameObject parent, GameObject go)
            where T : Component
        {
            return InstantiateTo(parent, go).GetComponent<T>();
        }

        /// <summary>
        /// <para>Transform以下に紐付いている子のGameObjectを全て削除する</para>
        /// <para>【第1引数】削除したいTransform</para>
        /// </summary>
        public static void DeleteAllChildren(Transform parent)
        {
            List<Transform> transformList = new List<Transform>();
            foreach (Transform child in parent)
            {
                transformList.Add(child);
            }
            parent.DetachChildren();
            foreach (Transform child in transformList)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        public static void MoveAnimation<T>(T monoBehaviour, Vector3 toPosition, float reachedSecond, Action<T> onMoveCompleted = null) where T : MonoBehaviour
        {
            monoBehaviour.StartCoroutine(MoveAnimationCorutine<T>(monoBehaviour, toPosition, reachedSecond, onMoveCompleted));
        }

        private static IEnumerator MoveAnimationCorutine<T>(T monoBehaviour, Vector3 toPosition, float reachedSecond, Action<T> onMoveCompleted = null) where T : MonoBehaviour
        {
            Vector3 beforePostion = monoBehaviour.transform.position;
            float second = 0;
            while (second < reachedSecond)
            {
                monoBehaviour.transform.position = Vector3.Lerp(beforePostion, toPosition, second / reachedSecond);
                second += Time.deltaTime;
                yield return null;
            }
            if (onMoveCompleted != null)
            {
                onMoveCompleted(monoBehaviour);
            }
        }
    }
}