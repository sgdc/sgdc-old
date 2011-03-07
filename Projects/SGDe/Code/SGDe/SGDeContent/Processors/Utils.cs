using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline;
using System.IO;
using SGDeContent.DataTypes;

namespace SGDeContent.Processors
{
    internal class Utils
    {
        public static T ParseEnum<T>(string value, T defaultValue, ContentBuildLogger log) where T : struct
        {
            string[] values = value.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            T evalue = defaultValue;
            T temp;
            for (int i = 0; i < values.Length; i++)
            {
                if (!Enum.TryParse<T>(values[i].Trim(), out temp))
                {
                    log.LogWarning(null, null, Messages.Utils_InvalidEnum, typeof(T).Name, values[i].Trim());
                }
                else
                {
                    int val = ((int)((object)evalue));
                    val |= ((int)((object)temp));
                    evalue = (T)((object)val);
                }
            }
            return evalue;
        }

        public static T SetContentItems<T>(string filename, T content) where T : ContentItem
        {
            content.Identity = new ContentIdentity(filename);
            content.Name = Path.GetFileNameWithoutExtension(filename);
            return content;
        }

        public static ExternalReference<ProcessedContent> CompileExternal(string file, ContentProcessorContext context)
        {
            return CompileExternal<Content, ProcessedContent, ProcessedContent, SGDEImport, SGDEProcessor>(file, context);
        }

        public static ExternalReference<TResult> CompileExternal<TResult>(string file, ContentProcessorContext context)
        {
            return CompileExternal<Content, ProcessedContent, TResult, SGDEImport, SGDEProcessor>(file, context);
        }

        public static ExternalReference<TProcessed> CompileExternal<TImport, TProcessed>(string file, ContentProcessorContext context)
        {
            return CompileExternal<TImport, TProcessed, TProcessed, SGDEImport, SGDEProcessor>(file, context);
        }

        public static ExternalReference<TResult> CompileExternal<TImport, TProcessed, TResult>(string file, ContentProcessorContext context)
        {
            return CompileExternal<TImport, TProcessed, TResult, SGDEImport, SGDEProcessor>(file, context);
        }

        public static ExternalReference<TResult> CompileExternal<TImport, TProcessed, TResult, TImporter, TProcessor>(string file, ContentProcessorContext context)
        {
            ExternalReference<TImport> efile = new ExternalReference<TImport>(file);
            StringBuilder builder = new StringBuilder();
            builder.Append(Path.GetDirectoryName(GetRelitivePath(context, efile.Filename))); //Do this to get the relitive path as XNA sees it
            if (builder.Length > 0)
            {
                builder.Append('\\');
            }
            builder.Append(Path.GetFileNameWithoutExtension(file));
            return CompileExternal<TImport, TProcessed, TResult, TImporter, TProcessor>(efile, context, builder.ToString());
        }

        public static ExternalReference<TResult> CompileExternal<TImport, TProcessed, TResult, TImporter, TProcessor>(ExternalReference<TImport> file, ContentProcessorContext context, string assetName)
        {
            ExternalReference<TProcessed> ext = context.BuildAsset<TImport, TProcessed>(file, typeof(TProcessor).Name, null, typeof(TImporter).Name, assetName);
            ExternalReference<TResult> naoExt;
            if (ext.Identity == null)
            {
                naoExt = new ExternalReference<TResult>(ext.Filename);
            }
            else
            {
                naoExt = new ExternalReference<TResult>(ext.Filename, ext.Identity);
            }
            return naoExt;
        }

        private static string GetRelitivePath(ContentProcessorContext context, string absPath)
        {
            Type type = Type.GetType("Microsoft.Xna.Framework.Content.Pipeline.XnaContentProcessorContext, Microsoft.Xna.Framework.Content.Pipeline");
            if (context.GetType().IsAssignableFrom(type))
            {
                object buildCoordinator = type.GetField("buildCoordinator", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(context);
                type = Type.GetType("Microsoft.Xna.Framework.Content.Pipeline.BuildCoordinator, Microsoft.Xna.Framework.Content.Pipeline");
                return (string)type.GetMethod("GetRelativePath", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public).Invoke(buildCoordinator, new object[] { absPath });
            }
            type = Type.GetType("SGDeB.ContentPipelineFake.FakeContentProcessorContext, SGDeB");
            if (context.GetType().IsAssignableFrom(type))
            {
                //Simply return the path, it's Only used for ExternalReferences any way.
                return absPath;
            }
            throw new ArgumentException(Messages.Utils_ChangedBuild);
        }
    }
}
