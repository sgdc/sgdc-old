using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SGDE
{
    /// <summary>
    /// Base class for any and all classes which can be added to the scene graph
    /// </summary>
    public abstract class SceneNode// : ICloneable
    {
        internal Vector2 mTranslation;
        internal float mRotation;
        internal Vector2 mScale;
        private SceneNode mParent;
        private List<SceneNode> mChildren;

        /// <summary>
        /// Constructor for a scene node
        /// </summary>
        public SceneNode()
        {
            mTranslation = new Vector2(0, 0);
            mRotation = 0;
            mScale = new Vector2(1, 1);

            mParent = null;
            mChildren = new List<SceneNode>();
        }

        /// <summary>Sets the translation of the scene node</summary>
        /// <param name="translation">Translation vector</param>
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

        /// <summary>
        /// Set the rotation of this scene node.
        /// </summary>
        /// <param name="rotation">Degree measure for rotation.</param>
        public void SetRotation(float rotation)
        {
            Rotate(rotation - mRotation);
        }

        /// <summary>
        /// Change the rotation of this scene node.
        /// </summary>
        /// <param name="rotation">The deleta degree measure for rotation.</param>
        public virtual void Rotate(float rotation)
        {
            mRotation += rotation;

            foreach (SceneNode node in mChildren)
            {
                node.Rotate(rotation);
            }
        }

        /// <summary>
        /// Get the rotation of this scene node.
        /// </summary>
        /// <returns>The degree measure of this scene node.</returns>
        public float GetRotation()
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

        internal void CopyTo(ref SceneNode ent)
        {
            ent.mTranslation = this.mTranslation;
            ent.mScale = this.mScale;
            ent.mRotation = this.mRotation;
            //Don't copy over children
        }

        /// <summary>
        /// Get this node element as the specified type.
        /// </summary>
        /// <typeparam name="T">Type of object to get.</typeparam>
        /// <returns>This node as the specified type or the default value of that type.</returns>
        public virtual T GetAsType<T>()
        {
            return default(T);
        }
    }
}
