/**************************************************************************
*      File Name：CompositeTransformExtension.cs
*    Description：CompositeTransformExtension.cs class description...
*      Copyright：Copyright © 2020 LeoYang-Chuese. All rights reserved.
*        Creator：Leo Yang
*    Create Time：2020/12/15
*Project Address：https://github.com/LeoYang-Chuese/wif
**************************************************************************/


using System;
using System.Windows.Markup;
using System.Windows.Media;

namespace Frontier.Wif.Infrastructure.MarkupExtensions
{
    /// <summary>
    /// Defines the <see cref="CompositeTransformExtension" />
    ///     http://www.singulink.com/CodeIndex/post/getting-rid-of-ugly-transformgroup-blocks-in-wpf
    /// </summary>
    public class CompositeTransformExtension : MarkupExtension
    {
        #region Fields

        /// <summary>
        /// Defines the _rotate
        /// </summary>
        private readonly RotateTransform _rotate = new RotateTransform();

        /// <summary>
        /// Defines the _scale
        /// </summary>
        private readonly ScaleTransform _scale = new ScaleTransform();

        /// <summary>
        /// Defines the _skew
        /// </summary>
        private readonly SkewTransform _skew = new SkewTransform();

        /// <summary>
        /// Defines the _translate
        /// </summary>
        private readonly TranslateTransform _translate = new TranslateTransform();

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the CenterX
        /// Gets or sets the CenterX
        /// </summary>
        public double CenterX
        {
            get => _scale.CenterX;
            set
            {
                _scale.CenterX = value;
                _skew.CenterX = value;
                _rotate.CenterX = value;
            }
        }

        /// <summary>
        /// Gets or sets the CenterY
        /// Gets or sets the CenterY
        /// </summary>
        public double CenterY
        {
            get => _scale.CenterY;
            set
            {
                _scale.CenterY = value;
                _skew.CenterY = value;
                _rotate.CenterY = value;
            }
        }

        /// <summary>
        /// Gets or sets the Rotation
        /// Gets or sets the Rotation
        /// </summary>
        public double Rotation
        {
            get => _rotate.Angle;
            set => _rotate.Angle = value;
        }

        /// <summary>
        /// Gets or sets the ScaleX
        /// Gets or sets the ScaleX
        /// </summary>
        public double ScaleX
        {
            get => _scale.ScaleX;
            set => _scale.ScaleX = value;
        }

        /// <summary>
        /// Gets or sets the ScaleY
        /// Gets or sets the ScaleY
        /// </summary>
        public double ScaleY
        {
            get => _scale.ScaleY;
            set => _scale.ScaleY = value;
        }

        /// <summary>
        /// Gets or sets the SkewX
        /// Gets or sets the SkewX
        /// </summary>
        public double SkewX
        {
            get => _skew.AngleX;
            set => _skew.AngleX = value;
        }

        /// <summary>
        /// Gets or sets the SkewY
        /// Gets or sets the SkewY
        /// </summary>
        public double SkewY
        {
            get => _skew.AngleY;
            set => _skew.AngleY = value;
        }

        /// <summary>
        /// Gets or sets the TranslateX
        /// Gets or sets the TranslateX
        /// </summary>
        public double TranslateX
        {
            get => _translate.X;
            set => _translate.X = value;
        }

        /// <summary>
        /// Gets or sets the TranslateY
        /// Gets or sets the TranslateY
        /// </summary>
        public double TranslateY
        {
            get => _translate.Y;
            set => _translate.Y = value;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The ProvideValue
        /// </summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider" /></param>
        /// <returns>The <see cref="object" /></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var group = new TransformGroup();
            group.Children.Add(_scale);
            group.Children.Add(_skew);
            group.Children.Add(_rotate);
            group.Children.Add(_translate);
            return group;
        }

        #endregion
    }
}