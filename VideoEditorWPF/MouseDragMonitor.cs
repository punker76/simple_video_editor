﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace VideoEditorWPF
{
    /// <summary>
    /// Provides more comprehensive events for clicking and dragging on an object
    /// </summary>
    public class MouseDragMonitor
    {
        public delegate void DragEventHandler(DragEventArgs args);

        public event DragEventHandler DragStarted;
        public event DragEventHandler DragMoved;
        public event DragEventHandler DragReleased;

        private MouseButton buttonClicked;
        private UIElement elementWatched;

        private bool isDragging = false;
        private Point prevMousePos;

        public MouseDragMonitor(UIElement elementWatched)
        {
            this.elementWatched = elementWatched;

            //Subscribe to the UIElement's mouse events
            elementWatched.MouseDown += ElementWatched_MouseDown;
            elementWatched.MouseMove += ElementWatched_MouseMove;
            elementWatched.MouseUp += ElementWatched_MouseUp;
        }

        private void ElementWatched_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Don't do anything if we're already dragging
            if (isDragging)
                return;

            //Start dragging
            isDragging = true;
            buttonClicked = e.ChangedButton;
            prevMousePos = e.GetPosition(elementWatched);
            Mouse.Capture(elementWatched, CaptureMode.Element);     //Capture the mouse so the user can safely drag the mouse out of the watched object's bounds

            //Send the drag started event
            if (DragStarted != null)
                DragStarted(new DragEventArgs(buttonClicked, 0, 0));
        }

        private void ElementWatched_MouseMove(object sender, MouseEventArgs e)
        {
            //Don't go on if we're not currently dragging
            if (!isDragging)
                return;

            //Calculate the delta movement so we can put it in the event args
            Point currentMousePos = e.GetPosition(elementWatched);

            double deltaX = currentMousePos.X - prevMousePos.X;
            double deltaY = currentMousePos.Y - prevMousePos.Y;

            //Save the current mouse pos as the previous one, so we can compute the delta again next time
            prevMousePos = currentMousePos;

            //Fire the drag moved event
            if (DragMoved != null)
                DragMoved(new DragEventArgs(buttonClicked, deltaX, deltaY));
        }

        private void ElementWatched_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //Don't go on if the we're not dragging
            if (!isDragging)
                return;

            //Don't go on if the wrong button was released.  We only want to stop dragging if the button that was originally clicked releases.
            if (e.ChangedButton != buttonClicked)
                return;

            //Stop dragging
            isDragging = false;
            Mouse.Capture(elementWatched, CaptureMode.None);

            //Fire the drag stopped event
            if (DragReleased != null)
                DragReleased(new DragEventArgs(buttonClicked, 0, 0));
        }
    }

    public class DragEventArgs
    {
        public MouseButton button;
        public double deltaX;
        public double deltaY;

        public DragEventArgs(MouseButton button, double deltaX, double deltaY)
        {
            this.button = button;
            this.deltaX = deltaX;
            this.deltaY = deltaY;
        }
    }
}
