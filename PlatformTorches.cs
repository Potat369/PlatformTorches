using System;
using System.Reflection;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace PlatformTorches
{
	public class PlatformTorches : Mod
	{
		public override void Load()
		{
			IL_SmartCursorHelper.Step_Torch += HookStepTorch;
		}

		private void HookStepTorch(ILContext il)
		{
			try
			{
				var c = new ILCursor(il);
				
				if (c.TryGotoNext(i => i.MatchBrfalse(out _), i => i.MatchLdsfld(typeof(Main).GetField("tileNoAttach")), i => i.MatchLdloca(10)))
				{
					c.GotoNext();
					c.EmitLdloca(10);
					c.EmitCall(typeof(Tile).GetProperty("type", BindingFlags.NonPublic | BindingFlags.Instance).GetGetMethod(true));  // Get Tile.type
					c.EmitLdindU2();
					c.EmitLdcI4(TileID.Platforms);
					c.Emit(OpCodes.Ceq);
					c.EmitBrtrue(c.Next.Next.Next.Next.Next.Next.Next);
				}
				MonoModHooks.DumpIL(ModContent.GetInstance<PlatformTorches>(), il);
			}
			catch (Exception e)
			{
				MonoModHooks.DumpIL(ModContent.GetInstance<PlatformTorches>(), il);
			}
		}
	}
}
