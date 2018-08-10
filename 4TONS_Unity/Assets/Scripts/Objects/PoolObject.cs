using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class PoolObject : MonoBehaviour
    {

        public virtual void loadObject()
        {

        }

        //Virtual Reset function so it can be overriden in classes with specific needs
        public virtual void ResetObject()
        {

        }
        public void Destroy(float time = 0)
        {
            if (time == 0)
            {
                TerminateObjectFunctions();
                gameObject.SetActive(false);
            }
            else
                StartCoroutine(DestroyRoutine(time));
        }
        IEnumerator DestroyRoutine(float time = 0)
        {
            yield return new WaitForSeconds(time);
            TerminateObjectFunctions();
            gameObject.SetActive(false);
        }
        private void OnDisable()
        {
            TerminateObjectFunctions();
        }

        //override this as you see fit.
        public virtual void TerminateObjectFunctions()
        {

        }
    }
