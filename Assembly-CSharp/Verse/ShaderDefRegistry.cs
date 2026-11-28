using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Waylith.Rendering;

namespace Verse
{
	// Token: 0x020001B9 RID: 441
	public static class ShaderDefRegistry
	{
		// Token: 0x06000C38 RID: 3128 RVA: 0x000407D0 File Offset: 0x0003E9D0
		public static void Clear()
		{
			ShaderRegistry.Clear();
		}

		// Token: 0x06000C39 RID: 3129 RVA: 0x000407D8 File Offset: 0x0003E9D8
		public static void RegisterAll()
		{
			ShaderDefRegistry.RegisterShaderDirectories();
			foreach (ShaderDef def in DefDatabase<ShaderDef>.AllDefs)
			{
				ShaderDefRegistry.Register(def);
			}
			UnityEngine.Shader.MarkEngineRegistryReady();
		}

		// Token: 0x06000C3A RID: 3130 RVA: 0x0004082C File Offset: 0x0003EA2C
		public static void HotReload()
		{
			ShaderHotReloader shaderHotReloader = ShaderHotReloader.Current;
			if (shaderHotReloader == null)
			{
				return;
			}
			ShaderDefRegistry.RegisterShaderDirectories();
			foreach (ShaderDef shaderDef in DefDatabase<ShaderDef>.AllDefs)
			{
				if (shaderDef.shaderProgram != null)
				{
					Waylith.Rendering.Shader shader;
					if (ShaderRegistry.TryResolve(shaderDef.defName, out shader) && shader.Key == shaderDef.defName && shader.CanReloadFromSource)
					{
						ShaderHotReloader shaderHotReloader2 = shaderHotReloader;
						Waylith.Rendering.Shader shader2 = shader;
						ShaderProgramDefinition shaderProgram = shaderDef.shaderProgram;
						IEnumerable<string> shaderKeywords = shaderDef.shaderKeywords;
						PipelineState? pipelineState = new PipelineState?(shaderDef.shaderProgram.ToPipelineState());
						ShaderDef iconVersion = shaderDef.iconVersion;
						shaderHotReloader2.Redefine(shader2, shaderProgram, shaderKeywords, pipelineState, (iconVersion != null) ? iconVersion.defName : null);
					}
					else
					{
						ShaderDefRegistry.Register(shaderDef);
					}
				}
			}
			shaderHotReloader.RequestReload();
		}

		// Token: 0x06000C3B RID: 3131 RVA: 0x000408F8 File Offset: 0x0003EAF8
		public static void RegisterShaderDirectories()
		{
			ShaderHotReloader shaderHotReloader = ShaderHotReloader.Current;
			List<string> list = new List<string>();
			List<ModContentPack> runningModsListForReading = LoadedModManager.RunningModsListForReading;
			for (int i = runningModsListForReading.Count - 1; i >= 0; i--)
			{
				ModContentPack modContentPack = runningModsListForReading[i];
				bool flag;
				if (shaderHotReloader != null && shaderHotReloader.WatchContentShaders)
				{
					ModMetaData modMetaData = modContentPack.ModMetaData;
					flag = (modMetaData != null && modMetaData.Source == ContentSource.ModsFolder);
				}
				else
				{
					flag = false;
				}
				bool flag2 = flag;
				foreach (string path in modContentPack.foldersToLoadDescendingOrder)
				{
					string text = Path.Combine(path, "Shaders");
					if (Directory.Exists(text))
					{
						list.Add(text);
						if (flag2)
						{
							shaderHotReloader.Watch(text, null);
						}
					}
				}
			}
			ShaderSourceLoader.SetOverrideShaderDirectories(list);
		}

		// Token: 0x06000C3C RID: 3132 RVA: 0x000409D0 File Offset: 0x0003EBD0
		private static void Register(ShaderDef def)
		{
			if (((def != null) ? def.shaderProgram : null) == null)
			{
				return;
			}
			ShaderProgramDefinition shaderProgram = def.shaderProgram;
			string defName = def.defName;
			string defName2 = def.defName;
			ShaderProgramDefinition definition = shaderProgram;
			IEnumerable<string> shaderKeywords = def.shaderKeywords;
			MaterialLayout materialLayout = null;
			PipelineState? pipelineState = new PipelineState?(shaderProgram.ToPipelineState());
			ShaderDef iconVersion = def.iconVersion;
			ShaderRegistry.Register(new Waylith.Rendering.Shader(defName, defName2, definition, shaderKeywords, materialLayout, pipelineState, (iconVersion != null) ? iconVersion.defName : null, null, null));
		}
	}
}
