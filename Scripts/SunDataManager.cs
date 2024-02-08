using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using TMPro;

public class SunDataManager : MonoBehaviour
{
    [SerializeField] private Transform sunRot;
    [SerializeField] private Transform solarPanelBody;
    [SerializeField] private Transform solarPanelHead;

    private SunData sunData;

    // 날짜
    [SerializeField] private TMP_InputField dateInput;

    private void Awake()
    {
        sunData = new SunData();
        SunMove();
    }
    // Start is called before the first frame update
    void Start()
    {
        objRotate(sunData.altitudeFif, sunData.azimuthFif);
    }

    // 태양 위치 구현
    public void SunMove()
    {
        string url = "http://apis.data.go.kr/B090041/openapi/service/SrAltudeInfoService/getAreaSrAltudeInfo" +
            "?ServiceKey=dmhrSq%2BTqlzT%2BnZUeLs4aOLl034z1ORuIrI0GvJjb86PSCTT6ycLhKNmZXrGETGBOBftom48mqszKlqj%2FXMCug%3D%3D" +
            "&location=서울" +
            "&locdate=20150101";

        // 데이터를 형성할 문서 생성 및 파일읽기
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(url);

        // item 태그 안에 데이터들 가져오기
        XmlNodeList bodyList = xmlDoc.GetElementsByTagName("item");
        foreach (XmlNode node in bodyList)
        {
            // XmlNodeList에서 특정 XmlNode 태그를 받아와서 데이터 저장
            string altMeridian = node.SelectSingleNode("altitudeMeridian").InnerText;
            sunData.altitudeMeridian = Angle(altMeridian);

            string altNine = node.SelectSingleNode("altitude_09").InnerText;
            sunData.altitudeNine = Angle(altNine);
            Debug.Log("altitudeNine : " + sunData.altitudeNine);

            string aziNine = node.SelectSingleNode("azimuth_09").InnerText;
            sunData.azimuthNine = Angle(aziNine);
            Debug.Log("azimuthNine : " + sunData.azimuthNine);

            string altTwelve = node.SelectSingleNode("altitude_12").InnerText;
            sunData.altitudeTwelve = Angle(altTwelve);
            Debug.Log("altitudeTwelve : " + sunData.altitudeTwelve);

            string aziTwelve = node.SelectSingleNode("azimuth_12").InnerText;
            sunData.azimuthTwelve = Angle(aziTwelve);
            Debug.Log("azimuthTwelve : " + sunData.azimuthTwelve);

            string altFif = node.SelectSingleNode("altitude_15").InnerText;
            sunData.altitudeFif = Angle(altFif);
            Debug.Log("altitudeFif : " + sunData.altitudeFif);

            string aziFif = node.SelectSingleNode("azimuth_15").InnerText;
            sunData.azimuthFif = Angle(aziFif);
            Debug.Log("azimuthFif : " + sunData.azimuthFif);
        }
    }

    public void objRotate(int altitude, int azimuth)
    {
        sunRot.rotation = Quaternion.Euler(altitude, azimuth, 0);
        solarPanelBody.rotation = Quaternion.Euler(0, azimuth - 180, 0);
        solarPanelHead.rotation = Quaternion.Euler(45 - altitude, azimuth - 180, 0);
    }

    public int Angle(string angle)
    {
        // 문자열을 '˚'를 기준으로 나누고 첫 번째 부분을 선택합니다.
        string degree = angle.Split('˚')[0]; 
        int degreeAsInt = int.Parse(degree);
        return degreeAsInt;
    }
}
