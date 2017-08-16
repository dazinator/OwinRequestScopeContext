﻿using System;
using System.Collections.Generic;

namespace DavidLievrouw.OwinRequestScopeContext {
  public interface IOwinRequestScopeContextItems : IDictionary<string, object> {
    void Add(string key, IDisposable value);
    void Add(string key, IDisposable value, bool disposeWhenRequestIsCompleted);
  }
}