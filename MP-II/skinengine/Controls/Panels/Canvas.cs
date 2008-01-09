#region Copyright (C) 2007 Team MediaPortal

/*
    Copyright (C) 2007 Team MediaPortal
    http://www.team-mediaportal.com
 
    This file is part of MediaPortal II

    MediaPortal II is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    MediaPortal II is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with MediaPortal II.  If not, see <http://www.gnu.org/licenses/>.
*/

#endregion
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using SkinEngine.Controls.Visuals;
using Rectangle = System.Drawing.Rectangle;

namespace SkinEngine.Controls.Panels
{
  public class Canvas : Panel
  {
    public Canvas()
    {
    }
    public Canvas(Canvas v)
      : base(v)
    {
    }

    public override object Clone()
    {
      return new Canvas(this);
    }
    /// <summary>
    /// measures the size in layout required for child elements and determines a size for the FrameworkElement-derived class.
    /// </summary>
    /// <param name="availableSize">The available size that this element can give to child elements.</param>
    public override void Measure(Size availableSize)
    {
      _desiredSize = new System.Drawing.Size((int)Width, (int)Height);
      if (Width <= 0)
        _desiredSize.Width = (int)availableSize.Width - (int)(Margin.X + Margin.W);
      if (Height <= 0)
        _desiredSize.Height = (int)availableSize.Height - (int)(Margin.Y + Margin.Z);

      if (LayoutTransform != null)
      {
        ExtendedMatrix m = new ExtendedMatrix();
        LayoutTransform.GetTransform(out m);
        SkinContext.AddLayoutTransform(m);
      }
      Rectangle rect = new Rectangle(0, 0, 0, 0);
      foreach (UIElement child in Children)
      {
        if (!child.IsVisible) continue;
        child.Measure(_desiredSize);
        rect = Rectangle.Union(rect, new Rectangle(new Point((int)child.Position.X, (int)child.Position.Y), new Size((int)child.DesiredSize.Width, (int)child.DesiredSize.Height)));
      }
      if (Width > 0) rect.Width = (int)Width;
      if (Height > 0) rect.Height = (int)Height;
      _desiredSize = rect.Size;
      _desiredSize.Width += (int)(Margin.X + Margin.W);
      _desiredSize.Height += (int)(Margin.Y + Margin.Z);
      _originalSize = _desiredSize;

      if (LayoutTransform != null)
      {
        SkinContext.RemoveLayoutTransform();
      }
      base.Measure(availableSize);
    }

    /// <summary>
    /// Arranges the UI element
    /// and positions it in the finalrect
    /// </summary>
    /// <param name="finalRect">The final size that the parent computes for the child element</param>
    public override void Arrange(Rectangle finalRect)
    {
      _finalRect = new System.Drawing.Rectangle(finalRect.Location, finalRect.Size);
      Rectangle layoutRect = new Rectangle(finalRect.X, finalRect.Y, finalRect.Width, finalRect.Height);
      layoutRect.X += (int)(Margin.X);
      layoutRect.Y += (int)(Margin.Y);
      layoutRect.Width -= (int)(Margin.X + Margin.W);
      layoutRect.Height -= (int)(Margin.Y + Margin.Z);
      //SkinContext.FinalLayoutTransform.TransformRect(ref layoutRect);

      ActualPosition = new Microsoft.DirectX.Vector3(layoutRect.Location.X, layoutRect.Location.Y, 1.0f); ;
      ActualWidth = layoutRect.Width;
      ActualHeight = layoutRect.Height;

      if (LayoutTransform != null)
      {
        ExtendedMatrix m = new ExtendedMatrix();
        LayoutTransform.GetTransform(out m);
        SkinContext.AddLayoutTransform(m);
      }


      foreach (FrameworkElement child in Children)
      {
        if (!child.IsVisible) continue;
        Point p = new Point((int)(child.Position.X), (int)(child.Position.Y));
        SkinContext.FinalLayoutTransform.TransformPoint(ref p);
        p.X += (int)this.ActualPosition.X;
        p.Y += (int)this.ActualPosition.Y;

        Size s = child.DesiredSize;

        child.Arrange(new Rectangle(p, s));
      }
      if (LayoutTransform != null)
      {
        SkinContext.RemoveLayoutTransform();
      }
      _finalLayoutTransform = SkinContext.FinalLayoutTransform;
      base.PerformLayout();
      base.Arrange(layoutRect);
    }

  }
}
