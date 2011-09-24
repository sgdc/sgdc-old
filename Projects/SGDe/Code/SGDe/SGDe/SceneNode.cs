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
            mTranslation = Vector2.Zero;
            mScale = Vector2.One;
        }

        /// <summary>
        /// Sets the translation of the scene node.
        /// </summary>
        /// <param name="translation">Translation vector.</param>
        public void SetTranslation(Vector2 translation)
        {
            Translate(translation - mTranslation);
        }

        /// <summary>
        /// Translate the scene node.
        /// </summary>
        /// <param name="x">The delta 'x' coordinate.</param>
        /// <param name="y">The delta 'y' coordinate.</param>
        public void Translate(float x, float y)
        {
            Translate(new Vector2(x, y));
        }

        /// <summary>
        /// Translate the scene node.
        /// </summary>
        /// <param name="translation">Translation vector.</param>
        public virtual void Translate(Vector2 translation)
        {
            mTranslation += translation;

            if (mChildren != null)
            {
                foreach (SceneNode node in mChildren)
                {
                    node.Translate(translation);
                }
            }
        }

        /// <summary>
        /// Get the current translation of the scene node.
        /// </summary>
        /// <returns>The translation of the scene node.</returns>
        public Vector2 GetTranslation()
        {
            return mTranslation;
        }

        /// <summary>
        /// Set the rotation of this scene node.
        /// </summary>
        /// <param name="rotation">Radian measure for rotation.</param>
        public void SetRotation(float rotation)
        {
            Rotate(rotation - mRotation);
        }

        /// <summary>
        /// Change the rotation of this scene node.
        /// </summary>
        /// <param name="rotation">The delta radian measure for rotation.</param>
        public virtual void Rotate(float rotation)
        {
            mRotation += rotation;

            if (mChildren != null)
            {
                foreach (SceneNode node in mChildren)
                {
                    node.Rotate(rotation);
                }
            }
        }

        /// <summary>
        /// Get the rotation of this scene node.
        /// </summary>
        /// <returns>The radian measure of this scene node.</returns>
        public float GetRotation()
        {
            return mRotation;
        }

        /// <summary>
        /// Set the scale of the scene node.
        /// </summary>
        /// <param name="scale">The scale to set the scene node to.</param>
        public void SetScale(float scale)
        {
            SetScale(new Vector2(scale, scale));
        }

        /// <summary>
        /// Set the scale of the scene node.
        /// </summary>
        /// <param name="scale">The scale to set the scene node to.</param>
        public void SetScale(Vector2 scale)
        {
            Scale(scale - mScale);
        }

        /// <summary>
        /// Uniformly scale the scene node.
        /// </summary>
        /// <param name="scale">The delta to scale the scene node by.</param>
        public void Scale(float scale)
        {
            Scale(scale, scale);
        }

        /// <summary>
        /// Scale the scene node.
        /// </summary>
        /// <param name="x">The delta 'x' scale.</param>
        /// <param name="y">The delta 'y' scale.</param>
        public void Scale(float x, float y)
        {
            Scale(new Vector2(x, y));
        }

        /// <summary>
        /// Scale the scene node.
        /// </summary>
        /// <param name="scale">The delta scale of the scene node.</param>
        public virtual void Scale(Vector2 scale)
        {
            mScale *= scale;

            if (mChildren != null)
            {
                foreach (SceneNode node in mChildren)
                {
                    node.Scale(scale);
                }
            }
        }

        /// <summary>
        /// Get the scale of the scene node.
        /// </summary>
        /// <returns>The scale of the scene node.</returns>
        public Vector2 GetScale()
        {
            return mScale;
        }

        /// <summary>
        /// Set the parent of this scene node. Only use this if absolutely necessary. It is better to use <see cref="AddChild"/> and <see cref="RemoveChild"/>, they calles SetParent themselves.
        /// </summary>
        /// <param name="parent">The parent to set to for this scene node.</param>
        public void SetParent(SceneNode parent)
        {
            mParent = parent;
        }

        /// <summary>
        /// Get the parent of this scene node if it exists.
        /// </summary>
        /// <returns>The parent of this scene node.</returns>
        public SceneNode GetParent()
        {
            return mParent;
        }

        /// <summary>
        /// Add a child to this scene node.
        /// </summary>
        /// <param name="child">The child to add to this scene node.</param>
        public void AddChild(SceneNode child)
        {
            if (child != null)
            {
                if (mChildren == null)
                {
                    mChildren = new List<SceneNode>();
                }
                if (child.mParent != null)
                {
                    child.mParent.RemoveChild(child);
                }
                mChildren.Add(child);
                child.SetParent(this);
            }
        }

        /// <summary>
        /// Remove a child from this scene node.
        /// </summary>
        /// <param name="child">The child to remove from this scene node.</param>
        public void RemoveChild(SceneNode child)
        {
            if (child != null)
            {
                if (child.mParent == this)
                {
                    child.SetParent(null);
                    if (mChildren != null)
                    {
                        mChildren.Remove(child);
                        if (mChildren.Count == 0)
                        {
                            mChildren = null;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get a list a children this scene node contains.
        /// </summary>
        /// <returns>A list of children from this scene node.</returns>
        public SceneNode[] GetChildren()
        {
            if (mChildren == null)
            {
                return new SceneNode[0];
            }
            return mChildren.ToArray();
        }

        internal void CopyTo(ref SceneNode ent)
        {
            ent.mTranslation = this.mTranslation;
            ent.mScale = this.mScale;
            ent.mRotation = this.mRotation;
            //Don't copy over children... or should we?
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
