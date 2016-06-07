using System;
using OpenTK;
using OpenTK.Input;

namespace Template
{
    public class Camera
    {
        //Position of the camera and the direction it is facing.
        private Vector3 position;
        public Vector3 direction;
        //The normalized vector created by adding the camera position and direction and normalizing it.
        private Vector3 CameraVector;
        //The corners of the screen
        private Vector3 p0, p1, p2;
        //Variables for the matrix of the camera movement.
        private Vector3 up, right;

        //The speed of the movement.
        private float speed = 0.1f;

        //Aspectratio of the screen.
        private float aspectRatio;

        //Variables for the FOV
        private float FOV;
        private float FOVAlpha;
        private const float Rad = (float)Math.PI / 180;

        public Camera(int screenWidth, int screenHeight)
        {
            aspectRatio = screenWidth / (float)screenHeight;

            position = Vector3.Zero;
            direction = Vector3.UnitZ;
            FOVAlpha = 178;

            Update();
        }

        /*
         * All of the userinput exept for the escape key is handeled here.
         * W and S for moving the camera forward and backward along the z axis toward the direction the camera is facing.
         * A and D for moving the camera over the x axis.
         * Q and E for moving the camera over the y axis.
         * The mouse controlls are also here, they move the direction the camera is facing according to the delta X and Y created by the movement of the mouse.
         */
        public void HandleInput(bool mouseUpdated, Vector2 mouseDelta)
        {
            KeyboardState keyboard = Keyboard.GetState();
            direction = position + CameraVector;

            bool updated = false;

            if (keyboard[Key.W]) //Forwards
            {
                position += CameraVector * speed;
                updated = true;
            }
            if (keyboard[Key.A]) //Left
            {
                position -= right * speed;
                direction -= right * speed;
                updated = true;
            }
            if (keyboard[Key.S]) //Backwards
            {
                position -= CameraVector * speed;
                updated = true;
            }
            if (keyboard[Key.D]) //Right
            {
                position += right * speed;
                direction += right * speed;
                updated = true;
            }

            if (keyboard[Key.Q]) //Up
            {
                position += up * speed;
                direction += up * speed;
                updated = true;
            }
            if (keyboard[Key.E]) //Down
            {
                position -= up * speed;
                direction -= up * speed;
                updated = true;
            }

            /*
             * You can only look 90 degrees up or down, then the x and z coordinate get infinately close to 0 till a rounding error makes it 0.
             * This is caused by the lazy implementation of the formula used in Lecture 4, slide 37.
             */
            if (mouseUpdated) //Mousemovement
            {
                direction += right * mouseDelta.X * speed;
                direction -= up * mouseDelta.Y * speed;

                updated = true;
            }

            /*
             * Only if the user has given an input will the Update function be called.
             * Also, no matter how many inputs the user gives the calculation will only be done once every update.
             */
            if (updated)
            {
                Update();
            }
        }

        /*
         * The update function.
         * This is where the new corners of the screen are calculated so the raytracer can display the correct rays.
         * These calculations are based on the formula in Lecture 4, slide 29-33.
         */
        public void Update()
        {
            CameraVector = Vector3.Normalize(direction - position);
            up = Vector3.UnitY;
            right = Vector3.Cross(up, CameraVector);
            up = Vector3.Cross(CameraVector, right);

            float tan = (float)Math.Tan(0.5f * FOVAlpha);
            FOV = Math.Abs(1 / (tan * Rad));

            // Calculate the positions needed for the raytracer.
            Vector3 middle = position + FOV * CameraVector;
            p0 = middle - FOV * aspectRatio * right + FOV * up;
            p1 = middle + FOV * aspectRatio * right + FOV * up;
            p2 = middle - FOV * aspectRatio * right - FOV * up;
        }

        //Getters needed for the raytracer.
        public Vector3[] GetPoints
        {
            get { return new Vector3[] { p0, p1, p2 }; }
        }
        public Vector3 Position { get { return position; } }
    }
}