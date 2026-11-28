using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020020D8 RID: 8408
	public static class BaseGen
	{
		// Token: 0x170025EA RID: 9706
		// (get) Token: 0x0600D397 RID: 54167 RVA: 0x004E0CBC File Offset: 0x004DEEBC
		public static string CurrentSymbolPath
		{
			get
			{
				return BaseGen.currentSymbolPath;
			}
		}

		// Token: 0x0600D398 RID: 54168 RVA: 0x004E0CC4 File Offset: 0x004DEEC4
		public static void Reset()
		{
			BaseGen.rulesBySymbol.Clear();
			List<RuleDef> allDefsListForReading = DefDatabase<RuleDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				List<RuleDef> list;
				if (!BaseGen.rulesBySymbol.TryGetValue(allDefsListForReading[i].symbol, out list))
				{
					list = new List<RuleDef>();
					BaseGen.rulesBySymbol.Add(allDefsListForReading[i].symbol, list);
				}
				list.Add(allDefsListForReading[i]);
			}
		}

		// Token: 0x0600D399 RID: 54169 RVA: 0x004E0D38 File Offset: 0x004DEF38
		public static void Generate()
		{
			if (BaseGen.working)
			{
				Log.Error("Cannot call Generate() while already generating. Nested calls are not allowed.");
				return;
			}
			BaseGen.working = true;
			BaseGen.currentSymbolPath = "";
			BaseGen.globalSettings.ClearResult();
			try
			{
				if (BaseGen.symbolStack.Empty)
				{
					Log.Warning("Symbol stack is empty.");
				}
				else if (BaseGen.globalSettings.map == null)
				{
					Log.Error("Called BaseGen.Resolve() with null map.");
				}
				else
				{
					int num = BaseGen.symbolStack.Count - 1;
					int num2 = 0;
					while (!BaseGen.symbolStack.Empty)
					{
						num2++;
						if (num2 > 100000)
						{
							Log.Error("Error in BaseGen: Too many iterations. Infinite loop?");
							break;
						}
						SymbolStack.Element element = BaseGen.symbolStack.Pop();
						BaseGen.currentSymbolPath = element.symbolPath;
						if (BaseGen.symbolStack.Count == num)
						{
							BaseGen.globalSettings.mainRect = element.resolveParams.rect;
							num--;
						}
						using (Rand.Block(BaseGen.globalSettings.map.NextGenSeed))
						{
							try
							{
								BaseGen.Resolve(element);
							}
							catch (Exception ex)
							{
								string[] array = new string[6];
								array[0] = "Error while resolving symbol \"";
								array[1] = element.symbol;
								array[2] = "\" with params=";
								int num3 = 3;
								ResolveParams resolveParams = element.resolveParams;
								array[num3] = resolveParams.ToString();
								array[4] = "\n\nException: ";
								int num4 = 5;
								Exception ex2 = ex;
								array[num4] = ((ex2 != null) ? ex2.ToString() : null);
								Log.Error(string.Concat(array));
							}
						}
					}
				}
			}
			catch (Exception ex3)
			{
				string str = "Error in BaseGen: ";
				Exception ex4 = ex3;
				Log.Error(str + ((ex4 != null) ? ex4.ToString() : null));
			}
			finally
			{
				BaseGen.globalSettings.landingPadsGenerated = BaseGen.globalSettings.basePart_landingPadsResolved;
				BaseGen.working = false;
				BaseGen.symbolStack.Clear();
				BaseGen.globalSettings.Clear();
			}
		}

		// Token: 0x0600D39A RID: 54170 RVA: 0x004E0F64 File Offset: 0x004DF164
		private static void Resolve(SymbolStack.Element toResolve)
		{
			string symbol = toResolve.symbol;
			ResolveParams resolveParams = toResolve.resolveParams;
			BaseGen.tmpResolvers.Clear();
			List<RuleDef> list;
			if (BaseGen.rulesBySymbol.TryGetValue(symbol, out list))
			{
				for (int i = 0; i < list.Count; i++)
				{
					RuleDef ruleDef = list[i];
					for (int j = 0; j < ruleDef.resolvers.Count; j++)
					{
						SymbolResolver symbolResolver = ruleDef.resolvers[j];
						if (symbolResolver.CanResolve(resolveParams))
						{
							BaseGen.tmpResolvers.Add(symbolResolver);
						}
					}
				}
			}
			if (!BaseGen.tmpResolvers.Any<SymbolResolver>())
			{
				string str = "Could not find any RuleDef for symbol \"";
				string str2 = symbol;
				string str3 = "\" with any resolver that could resolve ";
				ResolveParams resolveParams2 = resolveParams;
				Log.Warning(str + str2 + str3 + resolveParams2.ToString());
				return;
			}
			SymbolResolver symbolResolver2 = BaseGen.tmpResolvers.RandomElementByWeight((SymbolResolver x) => x.selectionWeight);
			resolveParams.rect = resolveParams.rect.ClipInsideMap(BaseGen.globalSettings.map);
			symbolResolver2.Resolve(resolveParams);
		}

		// Token: 0x040085FD RID: 34301
		public static GlobalSettings globalSettings = new GlobalSettings();

		// Token: 0x040085FE RID: 34302
		public static SymbolStack symbolStack = new SymbolStack();

		// Token: 0x040085FF RID: 34303
		private static Dictionary<string, List<RuleDef>> rulesBySymbol = new Dictionary<string, List<RuleDef>>();

		// Token: 0x04008600 RID: 34304
		private static bool working;

		// Token: 0x04008601 RID: 34305
		private static string currentSymbolPath;

		// Token: 0x04008602 RID: 34306
		private const int MaxResolvedSymbols = 100000;

		// Token: 0x04008603 RID: 34307
		private static readonly List<SymbolResolver> tmpResolvers = new List<SymbolResolver>();
	}
}
