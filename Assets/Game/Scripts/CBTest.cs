using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YummyGame.CubeWar
{
    public class CBTest : MonoBehaviour
    {
        public bool B { get; set; }
        List<IndexInfo> tsl = new List<IndexInfo>();
        CreateMap createMap;

        // Start is called before the first frame update
        void Start()
        {
            //tsl.Add(new IndexInfo() { x = 1, y = 2 });
            //tsl.Add(new IndexInfo() { x = 2, y = 3 });

            //IndexInfo temp = new IndexInfo() { x = 3, y = 2 };
            //Debug.Log(tsl.Contains(temp));

            //temp.x = 1;
            //Debug.Log(tsl.Contains(temp));
            //Debug.Log(B);


            createMap = new CreateMap();
            createMap.Init(CubeWarManager.Instance,
                (GameObject go) =>
                {
                    return Instantiate(go);
                },
                (GridType type) =>
                {
                    return Consts.MapPrefabPath[type];
                }
                );
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
