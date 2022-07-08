using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNAMath 
{

   public static Vector2 jacobiMethod(Vector2 origin1, Vector2 origin2, Vector2 dir1, Vector2 dir2, Vector2 initEst, int iterations, float percentThresh)
    {

        float J1 = dir1.x * -1;
        float J2 = dir2.x;
        float J3 = dir1.y * -1;
        float J4 = dir2.y;

        // Debug.Log("J1: " + J1);
        // Debug.Log("J2: " + J2);
        // Debug.Log("J3: " + J3);
        // Debug.Log("J4: " + J4);

        float curS = initEst.x;
        float curT = initEst.y;
        float newS = 1;
        float newT = 1;

        // Debug.Log("F1: " + funcRoot(dir1.x, dir2.x, curS, curT, origin1.x, origin2.x));
        // Debug.Log("F2: " + funcRoot(dir1.y, dir2.y, curS, curT, origin1.y, origin2.y));

        for (int i = 0; i < iterations; i++)
        {
            //Solve using newton Raphson (Jacobi Variant method) 
            newS = curS - ((funcRoot(dir1.x, dir2.x, curS, curT, origin1.x, origin2.x) * J4 - funcRoot(dir1.y, dir2.y, curS, curT, origin1.y, origin2.y) * J2) / (J1 * J4 - J2 * J3));

            newT = curT - ((funcRoot(dir1.y, dir2.y, curS, curT, origin1.y, origin2.y) * J1 - funcRoot(dir1.x, dir2.x, curS, curT, origin1.x, origin2.x) * J3) / (J1 * J4 - J2 * J3));

            curS = newS;
            curT = newT;
        }

        //  Debug.Log("S: " + curS);
        // Debug.Log("T: " + curT);

        return new Vector2(newS, newT);
    }

    public static float funcRoot(float coefS, float coefT, float valS, float valT, float addition1, float addition2)
    {
        return (coefT * valT + addition2) - (coefS * valS + addition1);
    }

    public static Vector2 gaussSiedelVectors(Vector2 origin1, Vector2 origin2, Vector2 dir1, Vector2 dir2, Vector2 initEst, int iterations, float percentThresh)
    {
        double sOld;
        double tOld;

        double s = initEst.x;
        double t = initEst.y;

        int count = 0;


        float diffS = 100;
        float diffT = 100;

        for (int i = 0; i < iterations; i++)
        {
            sOld = s;
            tOld = t;
            //Solve for S 
            s = ((origin2.x - origin1.x) + dir2.x * t) / dir1.x;

            //Solve for T
            t = ((origin1.y - origin2.y) + dir1.y * s) / dir2.y;


            //Calculate percent dif S
            diffS = Mathf.Abs((((float)s - (float)sOld) / (float)s) * 100);
            //Calcualte percent dif T
            diffT = Mathf.Abs((((float)t - (float)tOld) / (float)t) * 100);

            //Debug.Log("Something here");
            //Debug.Log(diffS);
            // Debug.Log(diffT);

            // Debug.Log(s);
            // Debug.Log(t);

        }

        //Debug.Log("Done");
        // Debug.Log("Final S: " + s + " T: " + t);
        //Debug.Log("S: " + s);
        return new Vector2((float)s, (float)t);

    }

    public static bool offScreen(float Fac, bool horizontal, bool moreThan)
    {
        if (horizontal)
        {
            if (moreThan)
            {
                if (Input.mousePosition.x > Screen.width * Fac)
                {

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (Input.mousePosition.x < Screen.width * Fac)
                {

                    return true;
                }
                else
                {
                    return false;
                }
            }


        }
        else
        {

            if (moreThan)
            {
                if (Input.mousePosition.y > Screen.height * Fac)
                {

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (Input.mousePosition.y < Screen.height * Fac)
                {

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }

    public static Vector3 makeCorner(Vector3 corner, Vector3 dimensions)
    {
        return new Vector3(corner.x + dimensions.x, corner.y, corner.z + dimensions.z);

    }

    public static float snapToUnit(float val, float unit)
    {
        float multiply = 1 / unit;

        float tempNum = val * multiply;

        tempNum = Mathf.Round(tempNum);

        return tempNum * unit;

    }

    public static Vector3 determineCorner(Vector3 corner1, Vector3 corner2, int cornerNum)
    {

        //Point 2      Point 6          Point 3

        //Point 5      Point 9          Point 7

        //Point 1      Point 8          Point 4


        switch (cornerNum)
        {
            case 1:
                return new Vector3(Mathf.Min(corner1.x, corner2.x), corner1.y, Mathf.Min(corner1.z, corner2.z));
            case 2:
                return new Vector3(Mathf.Min(corner1.x, corner2.x), corner1.y, Mathf.Max(corner1.z, corner2.z));
            case 3:
                return new Vector3(Mathf.Max(corner1.x, corner2.x), corner1.y, Mathf.Max(corner1.z, corner2.z));
            case 4:
                return new Vector3(Mathf.Max(corner1.x, corner2.x), corner1.y, Mathf.Min(corner1.z, corner2.z));

            case 5:

                return determineCorner(corner1, corner2, 1) + new Vector3(0, 0, (determineCorner(corner1, corner2, 2).z - determineCorner(corner1, corner2, 1).z) / 2);


               
            case 6:
                return determineCorner(corner1, corner2, 2) + new Vector3((determineCorner(corner1, corner2, 3).x - determineCorner(corner1, corner2, 2).x) / 2, 0, 0);
               
            case 7:
                return determineCorner(corner1, corner2, 4) + new Vector3(0, 0, (determineCorner(corner1, corner2,3).z - determineCorner(corner1, corner2, 4).z) / 2);
            case 8:
                return determineCorner(corner1, corner2, 1) + new Vector3((determineCorner(corner1, corner2, 4).x - determineCorner(corner1, corner2, 1).x) / 2, 0, 0);
            case 9:
                return new Vector3(determineCorner(corner1, corner2, 8).x, corner1.y, determineCorner(corner1, corner2, 5).z);
                


            /*
                        case 5:

                            return new Vector3(Mathf.Min(corner1.x, corner2.x), corner1.y, Mathf.Max(corner1.z, corner2.z)/2);
                        case 6:
                            return new Vector3(Mathf.Max(corner1.x, corner2.x)/2, corner1.y, Mathf.Max(corner1.z, corner2.z));
                        case 7:
                            return new Vector3(Mathf.Max(corner1.x, corner2.x), corner1.y, Mathf.Max(corner1.z, corner2.z)/2);
                        case 8:
                            return new Vector3(Mathf.Max(corner1.x, corner2.x)/2, corner1.y, Mathf.Min(corner1.z, corner2.z) / 2);
            */

            default:
                return new Vector3(Mathf.Min(corner1.x, corner2.x), corner1.y, Mathf.Min(corner1.z, corner2.z));
        }

    }

    public static Vector3 houseSize(Vector3 corner1, Vector3 corner2)
    {

        Vector3 dif = determineCorner(corner1, corner2, 3) - determineCorner(corner1, corner2, 1);

        dif.y = corner1.y;

        return dif;
    }


    public static bool intersectVector(Vector3 pos, Vector3 dim, Vector3 corner1, Vector3 corner2)
    {

        // Debug.Log("Pos: " + pos);
        // Debug.Log("dim: " + dim);
        // Debug.Log("Corner1: " + corner1);
        // Debug.Log("Corner2: " + corner2);


        Vector3 roomCorner3 = determineCorner(pos, pos + dim, 3);

        //Diagonal vector
        Vector3 diag = roomCorner3 - pos;
        Vector3 dirVec = corner2 - corner1;
        //Debug.Log(dirVec);

        Vector2 diagOriginVec = new Vector2(pos.x, pos.z);
        Vector2 diagDirVec = new Vector2(diag.x, diag.z);

        Vector2 roomOriginVec = new Vector2(corner1.x, corner1.z);
        Vector2 roomDirVec = new Vector2(dirVec.x, dirVec.z);


        float maxS1 = (roomCorner3.x - diagOriginVec.x) / diagDirVec.x;
        float maxT1 = 0;

        if (roomDirVec.x == 0)
        {
            maxT1 = (corner2.z - corner1.z) / roomDirVec.y;
        }
        else
        {
            maxT1 = (corner2.x - corner1.x) / roomDirVec.x;
        }

        //  Debug.Log(maxS1);
       // Debug.Log(maxT1);

        Vector2 initEsti = diagOriginVec + (diagDirVec * (maxS1 / 2));
        //  Debug.Log("Estimation: " + initEsti);


        Vector2 vecMulti = DNAMath.jacobiMethod(diagOriginVec, roomOriginVec, diagDirVec, roomDirVec, initEsti, 10, 1);

        vecMulti.x = snapToUnit(vecMulti.x, Mathf.Pow(10, -5));
        vecMulti.y = snapToUnit(vecMulti.y, Mathf.Pow(10, -5));

        //Debug.Log("S: " + vecMulti.x);
       //  Debug.Log("T: " + vecMulti.y);

       


        // Debug.Log("Point: " + (diagOriginVec + (vecMulti.x * diagDirVec)));
        // Debug.Log("Point: " + (roomOriginVec + (vecMulti.y * roomDirVec)));

        if ((vecMulti.x > 0 && vecMulti.x < maxS1) && (vecMulti.y > 0 && vecMulti.y < maxT1))
        {

            // Debug.Log("Here1");
            //Inside the room
            return true;
        }
        else
        {
            return false;
        }
    }



   public static Vector3 alignToPoint(Vector3 point, Vector3 size, int pointNum)
    {
        //Point 2      Point 6          Point 3

        //Point 5      Point 9          Point 7

        //Point 1      Point 8          Point 4




        //Size vector is in scale units (100 = 1m) but / 2 because at 100 it's 2m wide because it's 100 each side
        Vector3 pos = Vector3.zero;

        switch (pointNum)
        {
            case 1:
                pos = point + size / 100;
                break;
            case 2:
                pos = point + new Vector3(size.x, size.y, -size.z) / 100;
                break;
            case 3:
                pos = point + new Vector3(-size.x, size.y, -size.z) / 100;
                break;
            case 4:
                pos = point + new Vector3(-size.x, size.y, size.z) / 100;
                break;
            case 5:
                pos = point + new Vector3(size.x, size.y, 0) / 100;
                break;
            case 6:
                pos = point + new Vector3(0, size.y, -size.z) / 100;
                break;
            case 7:
                pos = point + new Vector3(-size.x, size.y, 0) / 100;
                break;
            case 8:
                pos = point + new Vector3(0, size.y, size.z) / 100;
                break;
            case 9:
                pos = point + new Vector3(0, size.y, 0) / 100;
                break;
        }

        return pos;
    }


    public static Vector3 CalcEulerAngleRot(Vector3 CamPos, Vector3 ObjPos, Vector3 CamVec)
    {
        Vector3 Angles = new Vector3(0, 0, 0);
        Vector3 up = Vector3.up;
        Vector3 toObj = VecBetweenObj(CamPos, ObjPos);
        Vector3 projUpCam = Projection(up, CamVec);
        Vector3 perpCam = PerpendiculartoProjection(CamVec, projUpCam);
        Vector3 projUpObj = Projection(up, toObj);
        Vector3 perpObj = PerpendiculartoProjection(toObj, projUpObj);

        if (perpCam.Equals(Vector3.zero))
        {
            // Camvec and up are the same direction 
            perpCam = CamVec;
        }

        if (perpObj.Equals(Vector3.zero))
        {
            //toObj and up are the same direction 
            perpObj = CamVec;
        }

        Angles.x = CalcXRot(perpCam, perpObj, CamVec, toObj);
        Angles.y = CalcYRot(perpCam, perpObj, toObj);

        return Angles;
    }


    static float DotProduct(Vector3 vec1, Vector3 vec2)
    {
        return vec1.x * vec2.x + vec1.y * vec2.y + vec1.z * vec2.z;
    }

    static Vector3 VecBetweenObj(Vector3 start, Vector3 end)
    {
        return new Vector3(end.x - start.x, end.y - start.y, end.z - start.z);
    }

    static Vector3 Projection(Vector3 projector, Vector3 projected)
    {
        //Projector is bottom vector, 
        return (DotProduct(projected, projector) / DotProduct(projector, projector)) * projector;

    }

    static Vector3 PerpendiculartoProjection(Vector3 vec, Vector3 projection)
    {
        return vec - projection;
    }

    static float LengthofVec(Vector3 vec)
    {
        return Mathf.Sqrt(vec.x * vec.x + vec.y * vec.y + vec.z * vec.z);
    }

    static float CalcYRot(Vector3 perp1, Vector3 perp2, Vector3 Target)
    {
        float yRotAngle = 0;
        yRotAngle = SignofAngle(Target, perp1, perp2, 1);
        return yRotAngle;
    }

    static float CalcXRot(Vector3 perp1, Vector3 perp2, Vector3 View, Vector3 Target)
    {
        //Angle between Cam and perpendicular
        float Ang1 = SignofAngle(Target, View, perp1, 2);
        float Ang2 = SignofAngle(Target, Target, perp2, 2);
        float xRotAngle = Ang1 - Ang2;
        return xRotAngle;
    }

    static float VectorDotProduct(Vector3 a, Vector3 b)
    {

        if (DotProduct(a, b) / (LengthofVec(a) * LengthofVec(b)) >= 1)
        {

            return 0;
        }
        else
        {
            return RadtoDeg(Mathf.Acos(DotProduct(a, b) / (LengthofVec(a) * LengthofVec(b))));
        }
    }

    static float SignofAngle(Vector3 Target, Vector3 vec1, Vector3 vec2, int dim)
    {
        float multi = 1;
        switch (dim)
        {
            case 1: //x

                if (Target.x >= 0)
                {
                    multi = 1;
                }
                else
                {
                    multi = -1;
                }
                break;
            case 2: //y

                if (Target.y >= 0)
                {
                    multi = 1;
                }
                else
                {
                    multi = -1;
                }
                break;
            case 3: //z

                if (Target.z >= 0)
                {
                    multi = 1;
                }
                else
                {
                    multi = -1;
                }
                break;
        }


        return VectorDotProduct(vec1, vec2) * multi;
    }

    static float RadtoDeg(float rad)
    {
        return rad * (180 / Mathf.PI);
    }

   public static float calcAmplitude (float points, float coef, float finalTime)
    {
        //Assuming we go from 0 - finalTime 
        return (points * coef) / (-Mathf.Cos(coef * finalTime) + 1);

    }

    public static float sinEq (float A, float B, float C, float D, float x)
    {
        
        return A * Mathf.Sin(B * (x + C)) + D;
    }

    public static string accurateTime (int time)
    {
        if (time < 10)
        {
            return 0 + time.ToString();
        } else
        {
            return time.ToString();
        }
    }

    public static float timeExp (float x)
    {

        return 25* Mathf.Pow(2f, -((x/600f)-7.65f));


    }

    /*
    public static float expFunc (float A, float B, float C, float D, )
    {
        return A* Mathf.Exp()


    }
    */









}
