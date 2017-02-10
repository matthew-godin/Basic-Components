using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.IO;
using System.Globalization;


namespace XNAProject
{
    class TrackData
    {
        const string PATH = "../../../../AS3D_2016Content/", STRING_TAB = "\t", STRING_TAB_BETWEEN_ZEROS = "0\t0", STRING_MINUS_DOT = "-.", STRING_MINUS_ONE_DOT = "-1.", STRING_ZERO_MINUS_ONE_DOT = "0-1.", STRING_ZERO_MINUS = "0-", STRING_MINUS = "-", STRING_E_MINUS = "e-", STRING_ZERO = "0", STRING_DOT_TAB = ".\t", STRING_DOT_ZERO_TAB = ".0\t", STRING_TAB_DOT = "\t.", STRING_TAB_ZERO_DOT = "\t0.", STRING_MINUS_ZERO_DOT = "-0.", STRING_EMPTY = "";
        const int POSITION_SPHERE = 0, VECTOR_4_NUM_COMPONENTS = 4, Flag_COMPONENT = 0, SECOND_COMPONENT = 1, THIRD_COMPONENT = 2, FOURTH_COMPONENT = 3, JUMP_E_MINUS = 2, Flag_LETTER = 0, BASE_E_MAPLE = 10, NOT_FOONED = -1, E_MAPLE_NULL = 0, NUM_BREADCRUMBS = 4, NUM_CENTRAL_DOTS = 20, CUBE_DERIVATIVE_FACTOR = 3, SQUARE_DERIVATIVE_FACTOR = 2, DISTANCE_BETWEEN_BORDER_AND_TRACK_CENTER = 3, GRID_DIMENSION_MAPLE = 8, GRID_DIMENSION_GROONED = 256, FIRST_INDEX = 0;
        const char CHARACTER_TAB = '\t', CHARACTER_MINUS = '-', CHARACTER_DOT = '.', CHARACTER_COMMA = ',', NEW_LINE_CHARACTER = '\n';
        const bool INNDER_BORDER = true;

        const float QUARTER = 1 / 4f;
        const float ONE_TWENTIETH = 1 / 20f;
        const int SCALE_FACTOR = 256 / 8;
        const int TRACK_HALF_WIDTH = 8;

        List<Vector2> OuterBorder { get; set; }
        List<Vector2> InnerBorder { get; set; }
        List<Vector2> CubePoints { get; set; }
        List<Vector2> Breadcrumbs { get; set; }
        List<Vector4> RawDataX { get; set; }
        List<Vector4> RawDataY { get; set; }
        List<Vector2> CentralPoints { get; set; }


        List<Vector4> SplineListX { get; set; }
        List<Vector4> SplineListY { get; set; }

        public Vector2 AvatarPosition
        {
            get
            {
                return new Vector2(CubePoints[POSITION_SPHERE].X, CubePoints[POSITION_SPHERE].Y);
            }
        }

        public TrackData(string fileNameSplineX, string fileNameSplineY)
        {
            char commaToReplace = CHARACTER_COMMA, pointÀMettre = CHARACTER_DOT;
            string[] stringsX, stringsY;

            stringsX = CreateFormattedString(fileNameSplineX, commaToReplace, pointÀMettre);
            stringsY = CreateFormattedString(fileNameSplineY, commaToReplace, pointÀMettre);            
            RawDataX = SaveRawData(RawDataX, stringsX);
            RawDataY = SaveRawData(RawDataY, stringsY);
            SaveCubePoints();

            Breadcrumbs = SavePoints(Breadcrumbs, NUM_BREADCRUMBS);
            CentralPoints = SavePoints(CentralPoints, NUM_CENTRAL_DOTS);
            OuterBorder = SaveBorder(OuterBorder, !INNDER_BORDER, NUM_CENTRAL_DOTS);
            InnerBorder = SaveBorder(InnerBorder, INNDER_BORDER, NUM_CENTRAL_DOTS);

            OuterBorder.Add(new Vector2(OuterBorder[FIRST_INDEX].X, OuterBorder[FIRST_INDEX].Y));
            InnerBorder.Add(new Vector2(InnerBorder[FIRST_INDEX].X, InnerBorder[FIRST_INDEX].Y));
        }

        public List<Vector2> GetOuterBorder()
        {
            return MakeListDeepCopy(OuterBorder);
        }

        public List<Vector2> GetInnerBorder()
        {
            return MakeListDeepCopy(InnerBorder);
        }

        public List<Vector2> GetCubePoints()
        {
            return MakeListDeepCopy(CubePoints);
        }

        public List<Vector2> GetBreadcrumbs()
        {
            return MakeListDeepCopy(Breadcrumbs);
        }

        List<Vector2> MakeListDeepCopy(List<Vector2> previousList)
        {
            List<Vector2> currentList = new List<Vector2>();
            foreach (Vector2 v in previousList)
            {
                currentList.Add(new Vector2(v.X, v.Y));
            }

            return currentList;
        }

        List<Vector2> AdjustForNewGrid(List<Vector2> list, int previousGridDimension, int currentGridDimension)
        {
            int adjustmentFactor = currentGridDimension / previousGridDimension;

            for (int i = 0; i < list.Count; ++i)
            {
                list[i] *= adjustmentFactor;
            }

            return list;
        }

        List<Vector2> SaveBorder(List<Vector2> list, bool inner, int numPoints)
        {
            list = new List<Vector2>();

            for (int i = 0; i < CentralPoints.Count; ++i)
            {
                Vector2 vd = CentralPoints[i] - CentralPoints[i - 1 == -1 ? CentralPoints.Count - 1 : i - 1];
                Vector2 vo = (inner ? new Vector2(vd.Y, -vd.X) : new Vector2(-vd.Y, vd.X) * DISTANCE_BETWEEN_BORDER_AND_TRACK_CENTER);
                list.Add(CentralPoints[i] + vo);
            }
            return list;
        }

        List<Vector2> SavePoints(List<Vector2> list, int numPoints)
        {
            list = new List<Vector2>();
            for (int i = 0; i < RawDataX.Count; ++i)
            {
                list = SaveMultiplePoints(list, numPoints, i);
            }

            return list;
        }

        List<Vector2> SaveMultiplePoints(List<Vector2> list, int numPoints, int i)
        {
            float factor;

            for (int j = 0; j < numPoints; ++j)
            {
                factor = i + j / (float)numPoints;
                list.Add(new Vector2(RawDataX[i].X + RawDataX[i].Y * factor * factor * factor + RawDataX[i].Z * factor * factor + RawDataX[i].W * factor, RawDataY[i].X + RawDataY[i].Y * factor * factor * factor + RawDataY[i].Z * factor * factor + RawDataY[i].W * factor));
            }

            return list;
        }

        void SaveCubePoints()
        {
            CubePoints = new List<Vector2>();
            for (int i = 0; i < RawDataX.Count; ++i)
            {
                CubePoints.Add(new Vector2(RawDataX[i].X + RawDataX[i].Y * i * i * i + RawDataX[i].Z * i * i + RawDataX[i].W * i, RawDataY[i].X + RawDataY[i].Y * i * i * i + RawDataY[i].Z * i * i + RawDataY[i].W * i));
            }
        }

        string[] CreateFormattedString(string fileName, char oldChar, char newChar)
        {
            StreamReader streamReader = new StreamReader(PATH + fileName);
            string s = STRING_EMPTY;
            string[] numberStrings;
            char[] separator = { CHARACTER_TAB, NEW_LINE_CHARACTER };

            while (!streamReader.EndOfStream)
            {
                s += streamReader.ReadLine();
                s += NEW_LINE_CHARACTER;
            }
            streamReader.Close();
            s = s.Replace(oldChar, newChar);
            s = PlaceZerosBeforeOrAfter(s, newChar);
            numberStrings = s.Split(separator);

            return numberStrings;
        }

        string PlaceZerosBeforeOrAfter(string s, char character)
        {
            s = s.Replace(STRING_DOT_TAB, STRING_DOT_ZERO_TAB);
            s = s.Replace(STRING_TAB_DOT, STRING_TAB_ZERO_DOT);
            s = s.Replace(STRING_MINUS_DOT, STRING_MINUS_ZERO_DOT);

            return s;
        }

        List<Vector4> SaveRawData(List<Vector4> rawData, string[] strings)
        {
            rawData = new List<Vector4>();
            for (int i = 0; i < strings.Length - 1; i = i + 4)
            {
                rawData.Add(32 * new Vector4(float.Parse(strings[i], CultureInfo.InvariantCulture), float.Parse(strings[i+1], CultureInfo.InvariantCulture), float.Parse(strings[i+2], CultureInfo.InvariantCulture), float.Parse(strings[i+3], CultureInfo.InvariantCulture)));
            }

            return rawData;
        }

        void ConvertStrings(int i, string[] componentStrings, float[] componentValues, string[] strings)
        {
            for (int j = 0; j < VECTOR_4_NUM_COMPONENTS; ++j)
            {
                componentStrings[j] = strings[i * VECTOR_4_NUM_COMPONENTS + j];
                componentValues[j] = ConvertMapleStringToFloat(componentStrings[j]);
            }
        }

        float ConvertMapleStringToFloat(string componentString)
        {
            int indexE = componentString.IndexOf(STRING_E_MINUS);
            bool eFound = indexE != NOT_FOONED;
            int valE = eFound ? int.Parse(componentString.Remove(Flag_LETTER, indexE + JUMP_E_MINUS)) : E_MAPLE_NULL;
            string stringWithoutE = eFound ? componentString.Remove(indexE) : componentString;

            return float.Parse(stringWithoutE) / (float)Math.Pow(BASE_E_MAPLE, valE);
        }
    }
}