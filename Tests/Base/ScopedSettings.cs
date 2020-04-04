﻿using LinqToDB.DataProvider.Firebird;
using System;

namespace Tests
{
	public class Scope : IDisposable
	{
		private readonly Action _onExit;

		public Scope(Action onEnter, Action onExit)
		{
			onEnter();
			_onExit = onExit;
		}

		void IDisposable.Dispose()
		{
			_onExit();
		}
	}

	public class FirebirdQuoteMode : IDisposable
	{
		private readonly FirebirdIdentifierQuoteMode _oldMode;

		public FirebirdQuoteMode(FirebirdIdentifierQuoteMode mode)
		{
			_oldMode = FirebirdConfiguration.IdentifierQuoteMode;
			FirebirdConfiguration.IdentifierQuoteMode = mode;
		}

		void IDisposable.Dispose()
		{
			FirebirdConfiguration.IdentifierQuoteMode = _oldMode;
		}
	}
}
