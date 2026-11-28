using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Verse
{
	// Token: 0x0200033A RID: 826
	public class ModAssemblyHandler
	{
		// Token: 0x06001942 RID: 6466 RVA: 0x00097918 File Offset: 0x00095B18
		public ModAssemblyHandler(ModContentPack mod)
		{
			this.mod = mod;
		}

		// Token: 0x06001943 RID: 6467 RVA: 0x00097934 File Offset: 0x00095B34
		public void ReloadAll()
		{
			foreach (FileInfo fileInfo in from f in ModContentPack.GetAllFilesForModPreserveOrder(this.mod, "Assemblies/", (string e) => e.ToLower() == ".dll", null)
			select f.Item2)
			{
				Assembly assembly;
				try
				{
					assembly = Assembly.LoadFrom(fileInfo.FullName);
				}
				catch (Exception value)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Exception loading ");
					defaultInterpolatedStringHandler.AppendFormatted(fileInfo.Name);
					defaultInterpolatedStringHandler.AppendLiteral(": ");
					defaultInterpolatedStringHandler.AppendFormatted<Exception>(value);
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
					break;
				}
				if (this.AssemblyIsUsable(assembly))
				{
					GenTypes.ClearCache();
					this.loadedAssemblies.Add(assembly);
				}
			}
		}

		// Token: 0x06001944 RID: 6468 RVA: 0x00097A44 File Offset: 0x00095C44
		private bool AssemblyIsUsable(Assembly asm)
		{
			try
			{
				asm.GetTypes();
			}
			catch (ReflectionTypeLoadException ex)
			{
				StringBuilder stringBuilder = new StringBuilder();
				StringBuilder stringBuilder2 = stringBuilder;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(56, 2);
				defaultInterpolatedStringHandler.AppendLiteral("ReflectionTypeLoadException getting types in assembly ");
				defaultInterpolatedStringHandler.AppendFormatted(asm.GetName().Name);
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted<ReflectionTypeLoadException>(ex);
				stringBuilder2.AppendLine(defaultInterpolatedStringHandler.ToStringAndClear());
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("Loader exceptions:");
				if (ex.LoaderExceptions != null)
				{
					foreach (Exception value in ex.LoaderExceptions)
					{
						StringBuilder stringBuilder3 = stringBuilder;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(6, 1);
						defaultInterpolatedStringHandler2.AppendLiteral("   => ");
						defaultInterpolatedStringHandler2.AppendFormatted<Exception>(value);
						stringBuilder3.AppendLine(defaultInterpolatedStringHandler2.ToStringAndClear());
					}
				}
				Log.Error(stringBuilder.ToString());
				return false;
			}
			catch (Exception ex2)
			{
				string str = "Exception getting types in assembly ";
				string name = asm.GetName().Name;
				string str2 = ": ";
				Exception ex3 = ex2;
				Log.Error(str + name + str2 + ((ex3 != null) ? ex3.ToString() : null));
				return false;
			}
			return true;
		}

		// Token: 0x040014E6 RID: 5350
		private ModContentPack mod;

		// Token: 0x040014E7 RID: 5351
		public List<Assembly> loadedAssemblies = new List<Assembly>();
	}
}
