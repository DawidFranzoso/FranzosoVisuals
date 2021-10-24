using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.Window;
using System.Threading;

namespace FranzosoVisuals
{
    class FVWindow
    {
        private long lastRenderTime;
        public Rf<float> speed = 1f;
        public Rf<float> time = 0;
        public RenderWindow window;
        public List<Primitive> primitives = new List<Primitive>();
        public TransformationHandler transformationHandler = null;
        public List<Action> instructionQueue = new List<Action>();
        public int instructionIndex = 0;

        public void loop()
        {
            window.DispatchEvents();
            updateTime();
            applyTransformations();
            render();
        }

        void applyTransformations() { transformationHandler.applyTransform(); }

        void updateTime()
        {
            long t_temp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            long deltaTime_temp = t_temp - lastRenderTime;
            lastRenderTime = t_temp;

            time.value += deltaTime_temp * speed.value;
        }

        void render()
        {
            window.Clear();
            foreach (Primitive p in primitives)
                p.draw(this);
            window.Display();
        }

        void runNextInstruction()
        {
            if (instructionQueue.Count <= 0) return;

            instructionQueue[instructionIndex%instructionQueue.Count]();
            instructionIndex++;
        }

        public FVWindow(bool fullscreen_a = true, uint max_framerate_a = 120, uint antialiasing_level_a = 16)
        {
            ContextSettings s = new ContextSettings();
            s.AntialiasingLevel = antialiasing_level_a;

            window = new RenderWindow(VideoMode.DesktopMode, "Franzoso Visuals (Default)", fullscreen_a ? Styles.Fullscreen:Styles.Default, s);
            window.SetFramerateLimit(max_framerate_a);

            window.SetActive();

            window.Closed += (sender, e) => { window.Close(); };
            window.Resized += (sender, e) => { window.SetView(new View(new FloatRect(0, 0, window.Size.X, window.Size.Y))); };

            window.MouseButtonReleased += (sender,e) => runNextInstruction();

            lastRenderTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            transformationHandler = new TransformationHandler(time);
        }

        public void Add(Primitive p) { primitives.Add(p); }
        public void Add<T>(Transformation<T> t) where T : ITransformable<T> { transformationHandler.addTransform(t); }
        public void Add<T>(TransformationProperties<T> t) where T : ITransformable<T> { transformationHandler.addTransform(t, time); }
        public void Add(Action a) { instructionQueue.Add(a); }
    }
}
