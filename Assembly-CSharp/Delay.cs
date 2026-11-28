using System;
using System.Collections;
using UnityEngine;
using Verse;

// Token: 0x02000020 RID: 32
public static class Delay
{
	// Token: 0x060000EC RID: 236 RVA: 0x00008ED2 File Offset: 0x000070D2
	private static Coroutine StartCoroutine(IEnumerator coroutine)
	{
		if (!Delay.coroutineRunner)
		{
			Delay.coroutineRunner = new GameObject("CoroutineRunner").AddComponent<Delay.CoroutineRunner>();
		}
		return Delay.coroutineRunner.StartCoroutine(coroutine);
	}

	// Token: 0x060000ED RID: 237 RVA: 0x00008EFF File Offset: 0x000070FF
	public static void AfterNSeconds(float seconds, Action action)
	{
		Delay.StartCoroutine(Delay.Internal_AfterNSeconds(seconds, action));
	}

	// Token: 0x060000EE RID: 238 RVA: 0x00008F0E File Offset: 0x0000710E
	private static IEnumerator Internal_AfterNSeconds(float seconds, Action action)
	{
		Delay.<Internal_AfterNSeconds>d__4 <Internal_AfterNSeconds>d__ = new Delay.<Internal_AfterNSeconds>d__4(0);
		<Internal_AfterNSeconds>d__.seconds = seconds;
		<Internal_AfterNSeconds>d__.action = action;
		return <Internal_AfterNSeconds>d__;
	}

	// Token: 0x060000EF RID: 239 RVA: 0x00008F24 File Offset: 0x00007124
	public static void AfterNTicks(int ticks, Action action)
	{
		Delay.StartCoroutine(Delay.Internal_AfterNTicks(GenTicks.TicksGame + ticks, action));
	}

	// Token: 0x060000F0 RID: 240 RVA: 0x00008F39 File Offset: 0x00007139
	private static IEnumerator Internal_AfterNTicks(int targetTick, Action action)
	{
		Delay.<Internal_AfterNTicks>d__6 <Internal_AfterNTicks>d__ = new Delay.<Internal_AfterNTicks>d__6(0);
		<Internal_AfterNTicks>d__.targetTick = targetTick;
		<Internal_AfterNTicks>d__.action = action;
		return <Internal_AfterNTicks>d__;
	}

	// Token: 0x04000047 RID: 71
	private static Delay.CoroutineRunner coroutineRunner;

	// Token: 0x0200245D RID: 9309
	private class CoroutineRunner : MonoBehaviour
	{
	}
}
