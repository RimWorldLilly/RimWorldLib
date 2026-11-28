using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Verse
{
	// Token: 0x02000247 RID: 583
	public class MapFileCompressor : IExposable
	{
		// Token: 0x0600112B RID: 4395 RVA: 0x000626C4 File Offset: 0x000608C4
		public MapFileCompressor(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600112C RID: 4396 RVA: 0x000626D3 File Offset: 0x000608D3
		public void ExposeData()
		{
			DataExposeUtility.LookByteArray(ref this.compressedData, "compressedThingMap");
		}

		// Token: 0x0600112D RID: 4397 RVA: 0x000626E5 File Offset: 0x000608E5
		public void BuildCompressedString()
		{
			this.compressibilityDecider = new CompressibilityDecider(this.map);
			this.compressibilityDecider.DetermineReferences();
			this.compressedData = MapSerializeUtility.SerializeUshort(this.map, new Func<IntVec3, ushort>(this.HashValueForSquare));
		}

		// Token: 0x0600112E RID: 4398 RVA: 0x00062720 File Offset: 0x00060920
		private ushort HashValueForSquare(IntVec3 curSq)
		{
			ushort num = 0;
			foreach (Thing thing in this.map.thingGrid.ThingsAt(curSq))
			{
				if (thing.IsSaveCompressible())
				{
					if (num != 0)
					{
						string str = "Found two compressible things in ";
						IntVec3 intVec = curSq;
						string str2 = intVec.ToString();
						string str3 = ". The last was ";
						Thing thing2 = thing;
						Log.Error(str + str2 + str3 + ((thing2 != null) ? thing2.ToString() : null));
					}
					num = thing.def.shortHash;
				}
			}
			return num;
		}

		// Token: 0x0600112F RID: 4399 RVA: 0x000627BC File Offset: 0x000609BC
		public IEnumerable<Thing> ThingsToSpawnAfterLoad()
		{
			Dictionary<ushort, ThingDef> thingDefsByShortHash = new Dictionary<ushort, ThingDef>();
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				ThingDef value;
				if (thingDefsByShortHash.TryGetValue(thingDef.shortHash, out value))
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(52, 3);
					defaultInterpolatedStringHandler.AppendLiteral("Hash collision between ");
					defaultInterpolatedStringHandler.AppendFormatted<ThingDef>(thingDef);
					defaultInterpolatedStringHandler.AppendLiteral(" and  ");
					defaultInterpolatedStringHandler.AppendFormatted<ThingDef>(value);
					defaultInterpolatedStringHandler.AppendLiteral(": both have short hash ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(thingDef.shortHash);
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				else
				{
					thingDefsByShortHash.Add(thingDef.shortHash, thingDef);
				}
			}
			int major = ScribeMetaHeaderUtility.loadedGameVersionMajor;
			int minor = ScribeMetaHeaderUtility.loadedGameVersionMinor;
			List<Thing> loadables = new List<Thing>();
			MapSerializeUtility.LoadUshort(this.compressedData, this.map, delegate(IntVec3 c, ushort val)
			{
				if (val == 0)
				{
					return;
				}
				ThingDef thingDef2 = BackCompatibility.BackCompatibleThingDefWithShortHash_Force(val, major, minor);
				if (thingDef2 == null)
				{
					try
					{
						thingDef2 = thingDefsByShortHash[val];
					}
					catch (KeyNotFoundException)
					{
						ThingDef thingDef3 = BackCompatibility.BackCompatibleThingDefWithShortHash(val);
						if (thingDef3 != null)
						{
							thingDef2 = thingDef3;
							thingDefsByShortHash.Add(val, thingDef3);
						}
						else
						{
							Log.Error("Map compressor decompression error: No thingDef with short hash " + val.ToString() + ". Adding as null to dictionary.");
							thingDefsByShortHash.Add(val, null);
						}
					}
				}
				if (thingDef2 != null)
				{
					try
					{
						if (!thingDef2.saveCompressible)
						{
							Log.Warning("Tried loading non-compressible thing as compressed thing: " + thingDef2.defName);
						}
						else
						{
							Thing thing = ThingMaker.MakeThing(thingDef2, null);
							thing.SetPositionDirect(c);
							loadables.Add(thing);
						}
					}
					catch (Exception ex)
					{
						string str = "Could not instantiate compressed thing: ";
						Exception ex2 = ex;
						Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
					}
				}
			});
			return loadables;
		}

		// Token: 0x04000FBC RID: 4028
		private Map map;

		// Token: 0x04000FBD RID: 4029
		private byte[] compressedData;

		// Token: 0x04000FBE RID: 4030
		public CompressibilityDecider compressibilityDecider;
	}
}
