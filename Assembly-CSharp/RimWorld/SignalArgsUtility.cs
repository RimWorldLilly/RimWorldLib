using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200112A RID: 4394
	public static class SignalArgsUtility
	{
		// Token: 0x06006E28 RID: 28200 RVA: 0x00269E74 File Offset: 0x00268074
		public static bool TryGetLookTargets(SignalArgs args, string name, out LookTargets lookTargets)
		{
			if (args.TryGetArg<LookTargets>(name, out lookTargets))
			{
				return true;
			}
			Thing t;
			if (args.TryGetArg<Thing>(name, out t))
			{
				lookTargets = t;
				return true;
			}
			WorldObject o;
			if (args.TryGetArg<WorldObject>(name, out o))
			{
				lookTargets = o;
				return true;
			}
			GlobalTargetInfo target;
			if (args.TryGetArg<GlobalTargetInfo>(name, out target))
			{
				lookTargets = target;
				return true;
			}
			TargetInfo target2;
			if (args.TryGetArg<TargetInfo>(name, out target2))
			{
				lookTargets = target2;
				return true;
			}
			List<Thing> targets;
			if (args.TryGetArg<List<Thing>>(name, out targets))
			{
				lookTargets = targets;
				return true;
			}
			lookTargets = LookTargets.Invalid;
			return false;
		}
	}
}
