using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SGDE
{
    public class SceneNode
    {
        private Vector2 mTranslation;
        private ushort mRotation;
        private Vector2 mScale;
        private SceneNode mParent;
        private List<SceneNode> mChildren;

        public SceneNode()
        {
            mTranslation = new Vector2(0, 0);
            mRotation = 0;
            mScale = new Vector2(1, 1);

            mParent = null;
            mChildren = new List<SceneNode>();
        }

        public void SetTranslation(Vector2 translation)
        {
            Translate(translation.X - mTranslation.X, translation.Y - mTranslation.Y);
        }

        public virtual void Translate(float x, float y)
        {
            mTranslation.X += x;
            mTranslation.Y += y;

            foreach (SceneNode node in mChildren)
            {
                node.Translate(x, y);
            }
        }

        public Vector2 GetTranslation()
        {
            return mTranslation;
        }

        public void SetRotation(ushort rotation)
        {
            Rotate((ushort)(rotation - mRotation));
        }

        public virtual void Rotate(ushort rotation)
        {
            mRotation += rotation;

            foreach (SceneNode node in mChildren)
            {
                node.Rotate(rotation);
            }
        }

        public ushort GetRotation()
        {
            return mRotation;
        }

        public void SetScale(Vector2 scale)
        {
            Scale(scale - mScale);
        }

        public virtual void Scale(Vector2 scale)
        {
            mScale += scale;

            foreach (SceneNode node in mChildren)
            {
                node.Scale(scale);
            }
        }

        public Vector2 GetScale()
        {
            return mScale;
        }

        public void SetParent(SceneNode parent)
        {
            mParent = parent;
        }

        public SceneNode GetParent()
        {
            return mParent;
        }

        public void AddChild(SceneNode child)
        {
            mChildren.Add(child);
            child.SetParent(this);
        }

        public void RemoveChild(SceneNode child)
        {
            mChildren.Remove(child);
            child.SetParent(null);
        }

        public List<SceneNode> GetChildren()
        {
            return mChildren;
        }
    }
}
