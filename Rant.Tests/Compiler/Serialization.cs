﻿using System;
using System.IO;

using NUnit.Framework;

namespace Rant.Tests.Compiler
{
	[TestFixture]
	public class Serialization
	{
		private readonly RantEngine rant = new RantEngine();

		[TestCase(@"Test")]
		[TestCase(@"\100,c")]
		[TestCase(@"\100,c \100,d")]
		[TestCase(@"{Test}")]
		[TestCase(@"{A|B|C}")]
		[TestCase(@"[r:10]{A|B|C}")]
		[TestCase(@"[r:[n:5;10]]{A|B|C}")]
		[TestCase(@"[r:[n:5;10]]{[repeach][x:_;forward]{A|B|C}}")]
		[TestCase(@"[$[test]:[xpin:_][x:_;forward][after:[xstep:_]]{Hello|World}][$test] [$test]")]
		[TestCase(@"[r:10]{(2)A|([n:2;3])B}")]
		[TestCase(@"[q:This is a quote [q:with a quote] in it.]")]
		[TestCase(@"")]
		[TestCase(@"{A{B|C}|D{E|F}}")]
		public void SerializeAndExecute(string pattern)
		{
			var ms = new MemoryStream();
			var pgmSer = RantPattern.CompileString(pattern);
			pgmSer.SaveToStream(ms);
			ms.Seek(0, SeekOrigin.Begin);
			var pgmDeser = RantPattern.LoadStream("Test", ms);
			var resultSer = rant.Do(pgmSer, seed: 0).Main;
			var resultDeser = rant.Do(pgmDeser, seed: 0).Main;
			Console.WriteLine($"Original: '{resultSer}'");
			Console.WriteLine($"Deserialized: '{resultDeser}'");
			Assert.AreEqual(resultSer, resultDeser);
		}
	}
}