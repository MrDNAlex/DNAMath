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


    public static List<string> getAlphabet ()
    {
        List<string> idk = new List<string>();

        idk.Add("a");
        idk.Add("b");
        idk.Add("c");
        idk.Add("d");
        idk.Add("e");
        idk.Add("f");
        idk.Add("g");
        idk.Add("h");
        idk.Add("i");
        idk.Add("j");
        idk.Add("k");
        idk.Add("l");
        idk.Add("m");
        idk.Add("n");
        idk.Add("o");
        idk.Add("p");
        idk.Add("q");
        idk.Add("r");
        idk.Add("s");
        idk.Add("t");
        idk.Add("u");
        idk.Add("v");
        idk.Add("w");
        idk.Add("x");
        idk.Add("y");
        idk.Add("z");

        idk.Add("A");
        idk.Add("B");
        idk.Add("C");
        idk.Add("D");
        idk.Add("E");
        idk.Add("F");
        idk.Add("G");
        idk.Add("H");
        idk.Add("I");
        idk.Add("J");
        idk.Add("K");
        idk.Add("L");
        idk.Add("M");
        idk.Add("N");
        idk.Add("O");
        idk.Add("P");
        idk.Add("Q");
        idk.Add("R");
        idk.Add("S");
        idk.Add("T");
        idk.Add("U");
        idk.Add("V");
        idk.Add("W");
        idk.Add("X");
        idk.Add("Y");
        idk.Add("Z");

        return idk;

    }

    public static float getMeasurement (string inputS, float oldVal)
    {
        //Wrap this in a function
        //Make a function to add the alphabet
        List<string> letters = DNAMath.getAlphabet();

        //If it fails strip all the letters and then look at all the combinations of units
        string input = inputS;
        string numOn = inputS;

        string newText = "";

        //Clean the numOn
        foreach (char i in numOn)
        {
            bool number = true;

            for (int j = 0; j < letters.Count; j++)
            {
                if (letters[j] == i.ToString())
                {
                    number = false;
                    Debug.Log("Not a Number");
                    Debug.Log(i);
                }

                if (number == false)
                {
                    j = letters.Count;
                }
            }

            if (number)
            {
                newText = newText + i.ToString();
            }
            Debug.Log(newText);
        }

        float num = float.Parse(newText);


        if (num + "m" == input || num + "M" == input)
        {
            return num;
            
        }
        else
        {
            return oldVal;
            
        }

    }


    public static string convertToTimeFormat (float seconds)
    {
        //Convert to time 
        string timeVal = "";

        int hour;
        int min;
        int sec;
        float rem;

        hour = (int)seconds / 3600;
        //- hour * 3600
        rem = seconds % 3600;
        min = (int)rem / 60;
        //- min * 60
        rem = rem % 60;
        sec = (int)rem / 1;
        rem = rem % 1;

        if (hour > 0)
        {
            //Include hour
            timeVal = DNAMath.accurateTime(hour) + ":" + DNAMath.accurateTime(min) + ":" + DNAMath.accurateTime(sec) + "." + cutOff(string.Format("{0:0.00}", rem), 2, 4);

        }
        else
        {
            timeVal = DNAMath.accurateTime(min) + ":" + DNAMath.accurateTime(sec) + "." +  cutOff(string.Format("{0:0.00}", rem), 2, 4);
        }

        return timeVal;
        
    }

    public static bool compareVector (Vector3 Vec1, Vector3 Vec2)
    {

        if (snapToUnit(Vec1.x, 0.1f) == snapToUnit(Vec2.x, 0.1f))
        {
            if (snapToUnit(Vec1.y, 0.1f) == snapToUnit(Vec2.y, 0.1f))
            {
                if (snapToUnit(Vec1.z, 0.1f) == snapToUnit(Vec2.z, 0.1f))
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
                return false;
            }
        } else
        {
            return false;
        }

    }

    public static string cutOff (string str, int start, int end)
    {
        string newStr = "";


        for (int i = start; i < end; i ++)
        {
            newStr = newStr + str[i];
        }

        return newStr;

    }

    public static float calcSlope(float length, float finaltime)
    {
        return (2 * length) / Mathf.Pow(finaltime, 2);
    }

    public static float linEq(float slope, float x)
    {
        return slope * x;
    }


    //These ones only work on desktop and at least only in unity
    
    public static List<string> extractText2(string path, string fileName = "")
    {
        //Change this system to look if the past 2 lines were empty probably by looking if they were the same
        List<string> strs = new List<string>();
        StreamReader reader = new StreamReader(path + fileName);

        //Read first line to get number
        int LineNumber = int.Parse(reader.ReadLine());
        reader.ReadLine();

        for (int i = 0; i < LineNumber; i++)
        {
            strs.Add(reader.ReadLine());
            reader.ReadLine();
        }

        return strs;
    }
    
    /*
    public static List<string> extractImagePaths(string path)
    {
        List<string> images = new List<string>();
        DirectoryInfo dir = new DirectoryInfo(path);

        foreach (var file in dir.GetFiles("*.png"))
        {
            string filePath = removeStr(removeStr(replace(removeUpTo(file.FullName, "Resources"), "\\", "/"), "Resources/"), ".png");
            images.Add(filePath);
        }
        foreach (var file in dir.GetFiles("*.jpg"))
        {
            string filePath = removeStr(removeStr(replace(removeUpTo(file.FullName, "Resources"), "\\", "/"), "Resources/"), ".jpg");
            images.Add(filePath);
        }

        Debug.Log(images[0]);
        return images;
    }
    */
    

    public static List<string> extractText (string path)
    {

        var temp = Resources.Load(path).ToString().Split("\n");

        List<string> list = new List<string>();

        foreach (string i in temp)
        {
           // Debug.Log(i);
            list.Add(i);
        }

        return list;
    }

    public static List<string> extractImages (string path)
    {
       path = removeStr (removeUpTo(path, "Resources"), "Resources/");

        var temp = Resources.Load(path).ToString().Split("!!!"); //!!! is a custom Splitter

        List<string> list = new List<string>();

        foreach (string i in temp)
        {
            if (i.Contains(".png"))
            {
                list.Add(removeStr(removeStr(replace(removeUpTo(i, "Resources"), "\\", "/"), "Resources/"), ".png"));
            } else
            {
                list.Add(removeStr(removeStr(replace(removeUpTo(i, "Resources"), "\\", "/"), "Resources/"), ".jpg"));
            }
            
        }

        return list;
    }

    public static string removeUpTo(string input, string upTo)
    {
        bool completeWord = false;

        string newStr = "";

        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == upTo[0] && completeWord == false)
            {
                for (int j = 0; j < upTo.Length; j++)
                {
                    if (input[i + j] == upTo[j])
                    {
                        if (j == (upTo.Length - 1))
                        {
                            completeWord = true;
                        }
                    }
                    else
                    {
                        j = upTo.Length;
                    }
                }
            }

            if (completeWord)
            {
                newStr += input[i];
            }
        }


        if (newStr == "")
        {
            return input;
        } else
        {
            return newStr;
        }
        
    }

    public static string replace(string input, string remove, string replace)
    {
        string newStr = "";

        for (int i = 0; i < input.Length; i++)
        {
            bool completeWord = false;
            if (input[i] == remove[0] && completeWord == false)
            {
                for (int j = 0; j < remove.Length; j++)
                {
                    if (input[i + j] == remove[j])
                    {
                        if (j == (remove.Length - 1))
                        {
                            completeWord = true;
                        }
                    }
                    else
                    {
                        j = remove.Length;
                    }
                }
            }
            if (completeWord)
            {
                newStr += replace;
                i += remove.Length - 1;
            }
            else
            {
                newStr += input[i];
            }
        }
        if (newStr == "")
        {
            return input;
        }
        else
        {
            return newStr;
        }
    }

    public static string removeStr(string input, string remove)
    {
        return input.Replace(remove, "");
    }

    public static GameObject extractModel(string path)
    {
        return GameObject.Instantiate(Resources.Load(removeStr(removeStr(replace(removeUpTo(Resources.Load(removeStr(removeStr(replace(removeUpTo(path, "Resources"), "\\", "/"), "Resources/"), ".fbx")).ToString(), "Resources"), "\\", "/"), "Resources/"), ".fbx")) as GameObject);
    }

    public static GameObject extractModelFromDirPath (string path)
    {
        return GameObject.Instantiate(Resources.Load(removeStr(removeStr(replace(removeUpTo(path, "Resources"), "\\", "/"), "Resources/"), ".fbx")) as GameObject);
    }

    public static bool checkMobile()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                return true;
            case RuntimePlatform.IPhonePlayer:
                return true;
            case RuntimePlatform.BlackBerryPlayer:
                return true;
            default:
                return false;
        }
    }

    public static string delLastCharIf (string input, string chr)
    {
        if (input[input.Length-1].ToString() == chr)
        {
            Debug.Log("Same Char");
        }
        return "hello";
    }

    public static string removeLastxChars (string input, int num)
    {
        string newstr = "";

        for (int i = 0; i < input.Length - (1 + num); i ++)
        {
            newstr += input[i];
        }

        return newstr;
    }

    public static bool mouseClick ()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)  || (Input.touchCount > 0 && Input.GetTouch(0).deltaTime < 0.1f))
        {
            return true;
        } else
        {
            return false;
        }
    }

    public static bool mouseHold ()
    {
        if (Input.GetKey(KeyCode.Mouse0) || Input.touchCount > 0)
        {

            return true;
        }
        
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).deltaTime > 0.1f)
            {
                return true;
            } else
            {
                return false;
            }
        } else
        {
            return false;
        }

        
    }

    public static Vector2 getMousePos ()
    {
        if (checkMobile())
        {
            //Mobile

            if (Input.touchCount > 0)
            {
                return Input.GetTouch(0).position;
            } else
            {
                return new Vector2(0, 0);
            }

        }
        else
        {
            //Desktop
            return Input.mousePosition;
        }
    }

    public static void DrawLine(Vector3 start, Vector3 end, Color color, GameObject parent)
    {
        GameObject line = new GameObject("ThinLine");
        line.transform.localPosition = start;
        line.AddComponent<LineRenderer>();
        line.transform.parent = parent.transform;
        LineRenderer lr = line.GetComponent<LineRenderer>();
        lr.material.color = color;
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        lr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

    }

    public static void DrawSquare(Vector3 start, Vector3 end, GameObject parent, Color color)
    {
        //Corner 2                   Corner 3



        //Corner 1                   Corner 4

        Vector3 Corner1 = start;
        Vector3 Corner2 = start;
        Corner2.z = end.z;
        Vector3 Corner3 = end;
        Vector3 Corner4 = end;
        Corner4.z = Corner1.z;

        DrawLine(Corner1, Corner2, color, parent);
        DrawLine(Corner2, Corner3, color, parent);
        DrawLine(Corner3, Corner4, color, parent);
        DrawLine(Corner4, Corner1, color, parent);

    }

    public static Vector3 getGlobalPos(float unit = 0, float baseheight = 0, Text text = null)
    {
        Vector3 pos = Vector3.one;

        Ray ray = Camera.main.ScreenPointToRay(DNAMath.getMousePos());
       // text.text = 
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 20))
        {
            pos = raycastHit.point;
        }
        else
        {
            // pos = cursor.transform.position;
        }
        pos.y = baseheight;

        if (unit == 0)
        {
            return new Vector3(pos.x, pos.y, pos.z);
        }
        else
        {
            return new Vector3(DNAMath.snapToUnit(pos.x, unit), pos.y, DNAMath.snapToUnit(pos.z, unit));
        }
        

    }

    public static float getUnitConversion (string inputS, float oldVal)
    {
        //Wrap this in a function
        //Make a function to add the alphabet
        List<string> letters = DNAMath.getAlphabet();

        //If it fails strip all the letters and then look at all the combinations of units
        string input = inputS;
        string numOn = inputS;

        string newText = "";

        //Clean the numOn
        foreach (char i in numOn)
        {
            bool number = true;

            for (int j = 0; j < letters.Count; j++)
            {
                if (letters[j] == i.ToString())
                {
                    number = false;
                  //  Debug.Log("Not a Number");
                  //  Debug.Log(i);
                }

                if (number == false)
                {
                    j = letters.Count;
                }
            }

            if (number)
            {
                newText = newText + i.ToString();
            }
        }
        float num = float.Parse(newText);

        if (num >= 0)
        {
            if (num + "k" == input || num + "K" == input)
            {
                //x1000   Kila
                return num * 1000;

            }
            else if (num + "m" == input || num + "M" == input)
            {
                //x1 000 000  Mega
                return num * 1000000;
            }
            else
            {
                return oldVal;
            }
        } else
        {
            return oldVal;
        }

      

    }

    public static string unitToString (float value, string unit)
    {
        string str = "";
        if (value >= 1000000)
        {
            str = ((float)value / 1000000f).ToString() + "M" + unit;
        } else if (value >= 1000)
        {
            str = ((float)value / 1000f).ToString() + "K" + unit;
        } else
        {
            str = value.ToString() + unit;
        }

       
        return str;


    }







}
