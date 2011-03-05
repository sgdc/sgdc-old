using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SGDE.Graphics;

namespace SGDE.Content.Readers
{
    /// <summary>
    /// Read and process a SceneNode class
    /// </summary>
    internal class NodeReader : ContentTypeReader<SceneNode>
    {
        /// <summary>
        /// Read a SceneNode.
        /// </summary>
        protected override SceneNode Read(ContentReader input, SceneNode existingInstance)
        {
            if (existingInstance == null)
            {
                throw new ContentLoadException(Messages.NodeReader_SceneNodeAbstract);
            }
            if (input.ReadBoolean())
            {
                existingInstance.mTranslation = input.ReadVector2();
            }
            else
            {
                SGDE.Content.Code.Code code = input.ReadRawObject<SGDE.Content.Code.Code>();
                float val = (float)code.Evaluate();
                code = input.ReadRawObject<SGDE.Content.Code.Code>();
                existingInstance.mTranslation = new Vector2(val, (float)code.Evaluate());
            }
            existingInstance.mRotation = input.ReadSingle();
            if (input.ReadBoolean())
            {
                existingInstance.mScale = input.ReadVector2();
            }
            else
            {
                SGDE.Content.Code.Code code = input.ReadRawObject<SGDE.Content.Code.Code>();
                float val = (float)code.Evaluate();
                code = input.ReadRawObject<SGDE.Content.Code.Code>();
                existingInstance.mScale = new Vector2(val, (float)code.Evaluate());
            }
            int childCount = input.ReadInt32();
            for (int i = 0; i < childCount; i++)
            {
                input.ReadSharedResource<SceneNode>(delegate(SceneNode nNode)
                {
                    existingInstance.AddChild(nNode);
                });
            }
            return existingInstance;
        }

        /// <summary>
        /// Can deserialize into an existing object. Have to do that for Nodes.
        /// </summary>
        public override bool CanDeserializeIntoExistingObject
        {
            get
            {
                return true;
            }
        }
    }
}
