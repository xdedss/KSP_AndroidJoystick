﻿using System;
using System.Collections.Generic;
using UnityEngine;

static class RunwayManager
{
    public static List<Runway> runways = new List<Runway>();

    public static void Init()
    {
        runways.Add(new Runway("Kerbin 600000 KSC -0.0487433327761511 -74.7045260278729 70.2358580782311 90 5 10000"));
    }

    public static Runway FindNearestRunway(double lat, double lon, string bodyName)
    {
        Runway nearest = null;
        double nearestDistance = 100000;
        foreach(Runway runway in runways)
        {
            if (runway.bodyName == bodyName)
            {
                double distance = SphericDistance(lat, lon, runway.lat, runway.lon, runway.seaLevel);
                Debug.Log("distance" + distance + " angle" + SphericAngle(lat, lon, runway.lat, runway.lon));
                if (distance < nearestDistance && distance < runway.length)
                {
                    nearest = runway;
                    nearestDistance = distance;
                }
            }
        }
        return nearest;
    }

    public static double SphericAngleCos(double lat1, double lon1, double lat2, double lon2)
    {
        return Math.Cos(lat1 * Mathf.Deg2Rad) * Math.Cos(lat2 * Mathf.Deg2Rad) * Math.Cos(lon1 * Mathf.Deg2Rad - lon2 * Mathf.Deg2Rad) + Math.Sin(lat1 * Mathf.Deg2Rad) * Math.Sin(lat2 * Mathf.Deg2Rad);
    }

    public static double SphericAngle(double lat1, double lon1, double lat2, double lon2)
    {
        return Math.Acos(SphericAngleCos(lat1, lon1, lat2, lon2));
    }

    public static double SphericDistance(double lat1, double lon1, double lat2, double lon2, double sealevel)
    {
        return sealevel * SphericAngle(lat1, lon1, lat2, lon2);
    }

    public class Runway
    {
        public string bodyName;
        public double seaLevel;
        public string siteName;
        public double lat;//of touchdown point degrees
        public double lon;
        public float altSL;//from sealevel
        public float angle;//rad,landing direction
        public float slope;//rad
        public float length;//meter

        //public Runway(string bodyName, string siteName, double lat, double lon, float angle, float slope, float length)
        //{
        //    this.bodyName = bodyName;
        //    this.siteName = siteName;
        //    this.lat = lat;
        //    this.lon = lon;
        //    this.angle = angle;
        //    this.slope = slope;
        //    this.length = length;
        //}

        public Runway(string data)
        {
            string[] datas = data.Split(' ');
            this.bodyName = datas[0];
            this.seaLevel = double.Parse(datas[1]);
            this.siteName = datas[2];
            this.lat = double.Parse(datas[3]);
            this.lon = double.Parse(datas[4]);
            this.altSL = float.Parse(datas[5]);
            this.angle = float.Parse(datas[6]) * Mathf.Deg2Rad;
            this.slope = float.Parse(datas[7]) * Mathf.Deg2Rad;
            this.length = float.Parse(datas[8]);
        }

        public Quaternion GetRotation()
        {
            return Quaternion.Euler((float)-lat, (float)-lon, angle * Mathf.Rad2Deg);
        }

        public Vector3 GetPosition()//z+to(0,0),x+to(0,-90),y+to(90,*)
        {
            Vector3 dir00 = new Vector3(0, 0, (float)seaLevel + altSL);
            return GetRotation() * dir00;
        }

        public void GetLandingFrame(out Vector3 LandingDir, out Vector3 LandingRight)
        {
            var rot = GetRotation();
            LandingDir = rot * Vector3.up;
            LandingRight = rot * Vector3.left;
        }

        public void AssessBias(double lat_, double lon_, float altSL_, out float horTan, out float verTan)
        {
            Vector3 landingDir;
            Vector3 LandingRight;
            GetLandingFrame(out landingDir, out LandingRight);
            var planeRot = Quaternion.Euler((float)-lat_, (float)-lon_, 0);
            var planePos = planeRot * new Vector3(0, 0, (float)seaLevel + altSL_);
            var directionalDistance = -Vector3.Dot(planePos, landingDir);
            var tangentDistance = Vector3.Dot(planePos, LandingRight);//right = positive;
            float properAltSL = altSL + Mathf.Tan(slope) * directionalDistance;
            var altitudeDistance = altSL_ - properAltSL;//too high = positive;

            horTan = tangentDistance / directionalDistance;
            verTan = altitudeDistance / directionalDistance;
        }
        
    }
}

