﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace gallery
{
    public partial class popupPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        public popupPage(string param)
        {
            InitializeComponent();
            img.Source = param.ToString();


        }
        private double _startScale, _currentScale;
        private double _startX, _startY;
        private double _xOffset, _yOffset;

        public double MinScale { get; set; } = 1;
        public double MaxScale { get; set; } = 3;

        

        protected override void OnSizeAllocated(double width, double height)
        {
            RestoreScaleValues();
            Content.AnchorX = 0.5;
            Content.AnchorY = 0.5;

            base.OnSizeAllocated(width, height);
        }

        private void RestoreScaleValues()
        {
            Content.ScaleTo(MinScale, 250, Easing.CubicInOut);
            Content.TranslateTo(0, 0, 250, Easing.CubicInOut);

            _currentScale = MinScale;
            _xOffset = Content.TranslationX = 0;
            _yOffset = Content.TranslationY = 0;
        }

        private void OnTapped(object sender, EventArgs e)
        {
            if (Content.Scale > MinScale)
            {
                RestoreScaleValues();
            }
            else
            {
                //todo: Add tap position somehow
                StartScaling();
                ExecuteScaling(MaxScale, .5, .5);
                EndGesture();
            }
        }

        private void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            switch (e.Status)
            {
                case GestureStatus.Started:
                    StartScaling();
                    break;

                case GestureStatus.Running:
                    ExecuteScaling(e.Scale, e.ScaleOrigin.X, e.ScaleOrigin.Y);
                    break;

                case GestureStatus.Completed:
                    RestoreScaleValues();
                    EndGesture();
                    break;
            }
        }

        private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    _startX = e.TotalX;
                    _startY = e.TotalY;

                    Content.AnchorX = 0;
                    Content.AnchorY = 0;

                    break;

                case GestureStatus.Running:
                    var maxTranslationX = Content.Scale * Content.Width - Content.Width;
                    Content.TranslationX = Math.Min(0, Math.Max(-maxTranslationX, _xOffset + e.TotalX - _startX));

                    var maxTranslationY = Content.Scale * Content.Height - Content.Height;
                    Content.TranslationY = Math.Min(0, Math.Max(-maxTranslationY, _yOffset + e.TotalY - _startY));

                    break;

                case GestureStatus.Completed:
                    EndGesture();
                    break;
            }
        }

        private void StartScaling()
        {
            _startScale = Content.Scale;

            Content.AnchorX = 0;
            Content.AnchorY = 0;
        }

        private void ExecuteScaling(double scale, double x, double y)
        {
            _currentScale += (scale - 1) * _startScale;
            _currentScale = Math.Max(MinScale, _currentScale);
            _currentScale = Math.Min(MaxScale, _currentScale);

            var deltaX = (Content.X + _xOffset) / Width;
            var deltaWidth = Width / (Content.Width * _startScale);
            var originX = (x - deltaX) * deltaWidth;

            var deltaY = (Content.Y + _yOffset) / Height;
            var deltaHeight = Height / (Content.Height * _startScale);
            var originY = (y - deltaY) * deltaHeight;

            var targetX = _xOffset - (originX * Content.Width) * (_currentScale - _startScale);
            var targetY = _yOffset - (originY * Content.Height) * (_currentScale - _startScale);

            Content.TranslationX = targetX.Clamp(-Content.Width * (_currentScale - 1), 0);
            Content.TranslationY = targetY.Clamp(-Content.Height * (_currentScale - 1), 0);

            Content.Scale = _currentScale;
        }

        private void EndGesture()
        {
            _xOffset = Content.TranslationX;
            _yOffset = Content.TranslationY;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }








        // ### Methods for supporting animations in your popup page ###

        // Invoked before an animation appearing
        protected override void OnAppearingAnimationBegin()
        {
            base.OnAppearingAnimationBegin();
        }

        // Invoked after an animation appearing
        protected override void OnAppearingAnimationEnd()
        {
            base.OnAppearingAnimationEnd();
        }

        // Invoked before an animation disappearing
        protected override void OnDisappearingAnimationBegin()
        {
            base.OnDisappearingAnimationBegin();
        }

        // Invoked after an animation disappearing
        protected override void OnDisappearingAnimationEnd()
        {
            base.OnDisappearingAnimationEnd();
        }

        protected override Task OnAppearingAnimationBeginAsync()
        {
            return base.OnAppearingAnimationBeginAsync();
        }

        protected override Task OnAppearingAnimationEndAsync()
        {
            return base.OnAppearingAnimationEndAsync();
        }

        protected override Task OnDisappearingAnimationBeginAsync()
        {
            return base.OnDisappearingAnimationBeginAsync();
        }

        protected override Task OnDisappearingAnimationEndAsync()
        {
            return base.OnDisappearingAnimationEndAsync();
        }

        // ### Overrided methods which can prevent closing a popup page ###

        // Invoked when a hardware back button is pressed
        protected override bool OnBackButtonPressed()
        {
            // Return true if you don't want to close this popup page when a back button is pressed
            return base.OnBackButtonPressed();
        }





        // Invoked when background is clicked
        protected override bool OnBackgroundClicked()
        {
            // Return false if you don't want to close this popup page when a background of the popup page is clicked
            return base.OnBackgroundClicked();
        }


    }
}