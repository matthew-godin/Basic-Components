//
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace XNAProject
{
    public class TexturedSphereAutomate : TexturedSphere
    {
        TrackData TrackData { get; set; }
        GroundWithBase Ground { get; set; }

        List<Vector2> BreadcrumbList { get; set; }
        Vector3[] ControlPts { get; set; }

        float t { get; set; }
        bool Pause { get; set; }

        Vector3 Direction { get; set; }
        Vector3 Position0 { get; set; }
        Vector3 PositionPlus1 { get; set; }
        Vector3 Displacement { get; set; }

        public TexturedSphereAutomate(Game game, float initialScale, Vector3 initialRotation, Vector3 initialPosition, float radius, Vector2 dimensions, string textureName, float updateInterval) : base(game, initialScale, initialRotation, initialPosition, radius, dimensions, textureName, updateInterval)
        { }

        public override void Initialize()
        {
            base.Initialize();
            Pause = false;
            t = 0;
            ControlPts = new Vector3[4];
            BreadcrumbList = TrackData.GetBreadcrumbs();

            Position0 = Ground.GetPointSpatial((int)BreadcrumbList[0].X, Ground.NumRows - (int)BreadcrumbList[0].Y);
            PositionPlus1 = Ground.GetPointSpatial((int)BreadcrumbList[1].X, Ground.NumRows - (int)BreadcrumbList[1].Y);
            Position = new Vector3(Position0.X, Position0.Y, Position0.Z);
            cpt = 1;
            AngleAxe = 0;

            Direction = PositionPlus1 - Position0;
            ControlPts = ComputeControlPoints();
        }

        protected override void LoadContent()
        {
            TrackData = Game.Services.GetService(typeof(TrackData)) as TrackData;
            Ground = Game.Services.GetService(typeof(GroundWithBase)) as GroundWithBase;
            base.LoadContent();
        }

        protected override void PerformUpdate()
        {
            if (Pause)
            {
                FollowBreadcrumbs();
                ManageRotationSphere();
                ManageCamera();
            }
            base.PerformUpdate();
        }

        int cpt { get; set; }

        void FollowBreadcrumbs() //comes here every 1/60 of a second
        {                       
            if (t > 60)//we want this to be true after 1 second, so 60 cycles
            {
                Position0 = Ground.GetPointSpatial((int)BreadcrumbList[cpt].X, Ground.NumRows - (int)BreadcrumbList[cpt].Y); //point where the ball is
                PositionPlus1 = Ground.GetPointSpatial((int)BreadcrumbList[cpt + 1].X, Ground.NumRows - (int)BreadcrumbList[cpt + 1].Y); //destination
                Direction = PositionPlus1 - Position0; //vector linking two points
                Position = Position0;
                ++cpt;
                if(cpt > BreadcrumbList.Count - 2)
                {
                    cpt = 0;
                }
                ControlPts = ComputeControlPoints(); //need new control points for the new points
                t = 0; //puts back t to 0 for a new cycle
            }
            //Position += Direction * 1f / 60f;          //if curve doesn't work
            Position = ComputeBezierCurve(t*(1f/60f), ControlPts);
            ++t;
        }

        private Vector3[] ComputeControlPoints()
        {
            Vector3[] pts = new Vector3[4];
            pts[0] = Position0;
            pts[3] = PositionPlus1;
            pts[1] = new Vector3(pts[0].X, pts[0].Y + 10, pts[0].Z);
            pts[2] = new Vector3(pts[3].X, pts[3].Y + 10, pts[3].Z);
            return pts;
        }

        private Vector3 ComputeBezierCurve(float t, Vector3[] ControlPts)
        {
            float x = (1 - t);
            return ControlPts[0] * (float)Math.Pow(x, 3) + 3 * ControlPts[1] * t * (float)Math.Pow(x, 2) + 3 * ControlPts[2] * t * t * x + ControlPts[3] * t * t * t;
        }

        void ManageCamera()
        {
            Vector3 displacement = Vector3.Normalize(Direction) * -25 + Vector3.Up * 10;

            Vector3 cameraPosition = Position + displacement;

            GameCamera.Displace(cameraPosition, Displacement + Position, Vector3.Up);
        }

        protected override void ManageKeyboard()
        {
            base.ManageKeyboard();
            if (InputMgr.IsNewKey(Keys.Space))
            {
                Pause = !Pause;
            }
        }

        int AngleAxe { get; set; }

        void ManageRotationSphere()
        {
            ComputeWorldMatrix(Vector3.Cross(Vector3.Normalize(Direction), Vector3.Up), 6 * (--AngleAxe));
        }

        protected void ComputeWorldMatrix(Vector3 axe, float angle)
        {
            World = Matrix.Identity *
                    Matrix.CreateScale(Homothétie) *
                    Matrix.CreateFromAxisAngle(axe, MathHelper.ToRadians(angle)) *
                    Matrix.CreateTranslation(Position);
        }
    }
}
