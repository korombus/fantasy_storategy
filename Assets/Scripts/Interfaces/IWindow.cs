using System;

public interface IWindow
{
    public abstract void SetData(Func<bool> retFunc, CommonSys sys);
}
