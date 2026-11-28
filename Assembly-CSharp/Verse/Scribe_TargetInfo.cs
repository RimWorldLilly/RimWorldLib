using System;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000531 RID: 1329
	public static class Scribe_TargetInfo
	{
		// Token: 0x06002731 RID: 10033 RVA: 0x000F52BB File Offset: 0x000F34BB
		public static void Look(ref LocalTargetInfo value, string label)
		{
			Scribe_TargetInfo.Look(ref value, false, label, LocalTargetInfo.Invalid, false);
		}

		// Token: 0x06002732 RID: 10034 RVA: 0x000F52CB File Offset: 0x000F34CB
		public static void Look(ref LocalTargetInfo value, bool saveDestroyedThings, string label)
		{
			Scribe_TargetInfo.Look(ref value, saveDestroyedThings, label, LocalTargetInfo.Invalid, true);
		}

		// Token: 0x06002733 RID: 10035 RVA: 0x000F52DB File Offset: 0x000F34DB
		public static void Look(ref LocalTargetInfo value, string label, LocalTargetInfo defaultValue)
		{
			Scribe_TargetInfo.Look(ref value, false, label, defaultValue, false);
		}

		// Token: 0x06002734 RID: 10036 RVA: 0x000F52E8 File Offset: 0x000F34E8
		public static void Look(ref LocalTargetInfo value, bool saveDestroyedThings, string label, LocalTargetInfo defaultValue, bool preserveDefaultValues = false)
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				if (!value.Equals(defaultValue) || preserveDefaultValues)
				{
					if (value.Thing != null && Scribe_References.CheckSaveReferenceToDestroyedThing(value.Thing, label, saveDestroyedThings))
					{
						return;
					}
					Scribe.saver.WriteElement(label, value.ToString());
					return;
				}
			}
			else
			{
				if (Scribe.mode == LoadSaveMode.LoadingVars)
				{
					value = ScribeExtractor.LocalTargetInfoFromNode(Scribe.loader.curXmlParent[label], label, defaultValue);
					return;
				}
				if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
				{
					value = ScribeExtractor.ResolveLocalTargetInfo(value, label);
				}
			}
		}

		// Token: 0x06002735 RID: 10037 RVA: 0x000F537C File Offset: 0x000F357C
		public static void Look(ref TargetInfo value, string label)
		{
			Scribe_TargetInfo.Look(ref value, false, label, TargetInfo.Invalid, false);
		}

		// Token: 0x06002736 RID: 10038 RVA: 0x000F538C File Offset: 0x000F358C
		public static void Look(ref TargetInfo value, bool saveDestroyedThings, string label)
		{
			Scribe_TargetInfo.Look(ref value, saveDestroyedThings, label, TargetInfo.Invalid, true);
		}

		// Token: 0x06002737 RID: 10039 RVA: 0x000F539C File Offset: 0x000F359C
		public static void Look(ref TargetInfo value, string label, TargetInfo defaultValue)
		{
			Scribe_TargetInfo.Look(ref value, false, label, defaultValue, false);
		}

		// Token: 0x06002738 RID: 10040 RVA: 0x000F53A8 File Offset: 0x000F35A8
		public static void Look(ref TargetInfo value, bool saveDestroyedThings, string label, TargetInfo defaultValue, bool preserveDefaultValues = false)
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				if (!value.Equals(defaultValue) || preserveDefaultValues)
				{
					if (value.Thing != null && Scribe_References.CheckSaveReferenceToDestroyedThing(value.Thing, label, saveDestroyedThings))
					{
						return;
					}
					if (!value.HasThing && value.Cell.IsValid && (value.Map == null || !Find.Maps.Contains(value.Map)))
					{
						Scribe.saver.WriteElement(label, "null");
						return;
					}
					Scribe.saver.WriteElement(label, value.ToString());
					return;
				}
			}
			else
			{
				if (Scribe.mode == LoadSaveMode.LoadingVars)
				{
					value = ScribeExtractor.TargetInfoFromNode(Scribe.loader.curXmlParent[label], label, defaultValue);
					return;
				}
				if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
				{
					value = ScribeExtractor.ResolveTargetInfo(value, label);
				}
			}
		}

		// Token: 0x06002739 RID: 10041 RVA: 0x000F5485 File Offset: 0x000F3685
		public static void Look(ref GlobalTargetInfo value, string label)
		{
			Scribe_TargetInfo.Look(ref value, false, label, GlobalTargetInfo.Invalid, false);
		}

		// Token: 0x0600273A RID: 10042 RVA: 0x000F5495 File Offset: 0x000F3695
		public static void Look(ref GlobalTargetInfo value, bool saveDestroyedThings, string label)
		{
			Scribe_TargetInfo.Look(ref value, saveDestroyedThings, label, GlobalTargetInfo.Invalid, true);
		}

		// Token: 0x0600273B RID: 10043 RVA: 0x000F54A5 File Offset: 0x000F36A5
		public static void Look(ref GlobalTargetInfo value, string label, GlobalTargetInfo defaultValue)
		{
			Scribe_TargetInfo.Look(ref value, false, label, defaultValue, false);
		}

		// Token: 0x0600273C RID: 10044 RVA: 0x000F54B4 File Offset: 0x000F36B4
		public static void Look(ref GlobalTargetInfo value, bool saveDestroyedThings, string label, GlobalTargetInfo defaultValue, bool preserveDefaultValues = false)
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				if (!value.Equals(defaultValue) || preserveDefaultValues)
				{
					if (value.Thing != null && Scribe_References.CheckSaveReferenceToDestroyedThing(value.Thing, label, saveDestroyedThings))
					{
						return;
					}
					if (value.WorldObject != null && Scribe_References.CheckSaveReferenceToDestroyedWorldObject(value.WorldObject, label, saveDestroyedThings))
					{
						return;
					}
					if (!value.HasThing && !value.HasWorldObject && value.Cell.IsValid && (value.Map == null || !Find.Maps.Contains(value.Map)))
					{
						Scribe.saver.WriteElement(label, "null");
						return;
					}
					Scribe.saver.WriteElement(label, value.ToString());
					return;
				}
			}
			else
			{
				if (Scribe.mode == LoadSaveMode.LoadingVars)
				{
					value = ScribeExtractor.GlobalTargetInfoFromNode(Scribe.loader.curXmlParent[label], label, defaultValue);
					return;
				}
				if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
				{
					value = ScribeExtractor.ResolveGlobalTargetInfo(value, label);
				}
			}
		}
	}
}
