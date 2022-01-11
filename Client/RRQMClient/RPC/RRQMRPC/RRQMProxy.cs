using System;
using RRQMSocket.RPC;
using RRQMSocket.RPC.RRQMRPC;
using RRQMCore.Exceptions;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
namespace RRQMProxy
{
public interface IRpcServer:IRemoteServer
{
///<summary>
///测试同步调用
///</summary>
 System.String TestOne (System.Int32 id,InvokeOption invokeOption = null);
///<summary>
///测试同步调用
///</summary>
Task<System.String> TestOneAsync (System.Int32 id,InvokeOption invokeOption = null);

///<summary>
///测试TestTwo
///</summary>
 System.String TestTwo (System.Int32 id,InvokeOption invokeOption = null);
///<summary>
///测试TestTwo
///</summary>
Task<System.String> TestTwoAsync (System.Int32 id,InvokeOption invokeOption = null);

///<summary>
///测试重载调用
///</summary>
 System.String TestOne_Name (System.Int32 id,System.String name,InvokeOption invokeOption = null);
///<summary>
///测试重载调用
///</summary>
Task<System.String> TestOne_NameAsync (System.Int32 id,System.String name,InvokeOption invokeOption = null);

///<summary>
///测试Out
///</summary>
  void TestOut (out System.Int32 id,InvokeOption invokeOption = null);

///<summary>
///测试Ref
///</summary>
  void TestRef (ref System.Int32 id,InvokeOption invokeOption = null);

///<summary>
///测试异步
///</summary>
 System.String AsyncTestOne (System.Int32 id,InvokeOption invokeOption = null);
///<summary>
///测试异步
///</summary>
Task<System.String> AsyncTestOneAsync (System.Int32 id,InvokeOption invokeOption = null);

}
public class RpcServer :IRpcServer
{
public RpcServer(IRpcClient client)
{
this.Client=client;
}
public IRpcClient Client{get;private set; }
///<summary>
///<inheritdoc/>
///</summary>
public System.String TestOne (System.Int32 id,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{id};
System.String returnData=Client.Invoke<System.String>("TestOne",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
public  async Task<System.String> TestOneAsync (System.Int32 id,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
return await Task.Run(() =>{
return TestOne(id,invokeOption);});
}

///<summary>
///<inheritdoc/>
///</summary>
public System.String TestTwo (System.Int32 id,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{id};
System.String returnData=Client.Invoke<System.String>("TestTwo",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
public  async Task<System.String> TestTwoAsync (System.Int32 id,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
return await Task.Run(() =>{
return TestTwo(id,invokeOption);});
}

///<summary>
///<inheritdoc/>
///</summary>
public System.String TestOne_Name (System.Int32 id,System.String name,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{id,name};
System.String returnData=Client.Invoke<System.String>("TestOne_Name",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
public  async Task<System.String> TestOne_NameAsync (System.Int32 id,System.String name,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
return await Task.Run(() =>{
return TestOne_Name(id,name,invokeOption);});
}

///<summary>
///<inheritdoc/>
///</summary>
public  void TestOut (out System.Int32 id,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{default(System.Int32)};
Type[] types = new Type[]{typeof(System.Int32)};
Client.Invoke("TestOut",invokeOption,ref parameters,types);
if(parameters!=null)
{
id=(System.Int32)parameters[0];
}
else
{
id=default(System.Int32);
}
}

///<summary>
///<inheritdoc/>
///</summary>
public  void TestRef (ref System.Int32 id,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{id};
Type[] types = new Type[]{typeof(System.Int32)};
Client.Invoke("TestRef",invokeOption,ref parameters,types);
if(parameters!=null)
{
id=(System.Int32)parameters[0];
}
}

///<summary>
///<inheritdoc/>
///</summary>
public System.String AsyncTestOne (System.Int32 id,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{id};
System.String returnData=Client.Invoke<System.String>("AsyncTestOne",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
public  async Task<System.String> AsyncTestOneAsync (System.Int32 id,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
return await Task.Run(() =>{
return AsyncTestOne(id,invokeOption);});
}

}
public interface IPerformanceRpcServer:IRemoteServer
{
///<summary>
///测试性能
///</summary>
 System.String Performance (InvokeOption invokeOption = null);
///<summary>
///测试性能
///</summary>
Task<System.String> PerformanceAsync (InvokeOption invokeOption = null);

///<summary>
///测试并发性能
///</summary>
 System.Int32 ConPerformance (System.Int32 num,InvokeOption invokeOption = null);
///<summary>
///测试并发性能
///</summary>
Task<System.Int32> ConPerformanceAsync (System.Int32 num,InvokeOption invokeOption = null);

///<summary>
///测试并发性能2
///</summary>
 System.Int32 ConPerformance2 (System.Int32 num,InvokeOption invokeOption = null);
///<summary>
///测试并发性能2
///</summary>
Task<System.Int32> ConPerformance2Async (System.Int32 num,InvokeOption invokeOption = null);

}
public class PerformanceRpcServer :IPerformanceRpcServer
{
public PerformanceRpcServer(IRpcClient client)
{
this.Client=client;
}
public IRpcClient Client{get;private set; }
///<summary>
///<inheritdoc/>
///</summary>
public System.String Performance (InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{};
System.String returnData=Client.Invoke<System.String>("Performance",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
public  async Task<System.String> PerformanceAsync (InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
return await Task.Run(() =>{
return Performance(invokeOption);});
}

///<summary>
///<inheritdoc/>
///</summary>
public System.Int32 ConPerformance (System.Int32 num,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{num};
System.Int32 returnData=Client.Invoke<System.Int32>("ConPerformance",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
public  async Task<System.Int32> ConPerformanceAsync (System.Int32 num,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
return await Task.Run(() =>{
return ConPerformance(num,invokeOption);});
}

///<summary>
///<inheritdoc/>
///</summary>
public System.Int32 ConPerformance2 (System.Int32 num,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{num};
System.Int32 returnData=Client.Invoke<System.Int32>("ConPerformance2",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
public  async Task<System.Int32> ConPerformance2Async (System.Int32 num,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
return await Task.Run(() =>{
return ConPerformance2(num,invokeOption);});
}

}
public interface IElapsedTimeRpcServer:IRemoteServer
{
///<summary>
///测试可取消的调用
///</summary>
 System.Boolean DelayInvoke (System.Int32 tick,InvokeOption invokeOption = null);
///<summary>
///测试可取消的调用
///</summary>
Task<System.Boolean> DelayInvokeAsync (System.Int32 tick,InvokeOption invokeOption = null);

}
public class ElapsedTimeRpcServer :IElapsedTimeRpcServer
{
public ElapsedTimeRpcServer(IRpcClient client)
{
this.Client=client;
}
public IRpcClient Client{get;private set; }
///<summary>
///<inheritdoc/>
///</summary>
public System.Boolean DelayInvoke (System.Int32 tick,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{tick};
System.Boolean returnData=Client.Invoke<System.Boolean>("DelayInvoke",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
public  async Task<System.Boolean> DelayInvokeAsync (System.Int32 tick,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
return await Task.Run(() =>{
return DelayInvoke(tick,invokeOption);});
}

}
public interface IInstanceRpcServer:IRemoteServer
{
///<summary>
///测试调用实例
///</summary>
 System.Int32 Increment (InvokeOption invokeOption = null);
///<summary>
///测试调用实例
///</summary>
Task<System.Int32> IncrementAsync (InvokeOption invokeOption = null);

}
public class InstanceRpcServer :IInstanceRpcServer
{
public InstanceRpcServer(IRpcClient client)
{
this.Client=client;
}
public IRpcClient Client{get;private set; }
///<summary>
///<inheritdoc/>
///</summary>
public System.Int32 Increment (InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{};
System.Int32 returnData=Client.Invoke<System.Int32>("Increment",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
public  async Task<System.Int32> IncrementAsync (InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
return await Task.Run(() =>{
return Increment(invokeOption);});
}

}
public interface IGetCallerRpcServer:IRemoteServer
{
///<summary>
///测试调用上下文
///</summary>
 System.String GetCallerID (InvokeOption invokeOption = null);
///<summary>
///测试调用上下文
///</summary>
Task<System.String> GetCallerIDAsync (InvokeOption invokeOption = null);

}
public class GetCallerRpcServer :IGetCallerRpcServer
{
public GetCallerRpcServer(IRpcClient client)
{
this.Client=client;
}
public IRpcClient Client{get;private set; }
///<summary>
///<inheritdoc/>
///</summary>
public System.String GetCallerID (InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{};
System.String returnData=Client.Invoke<System.String>("GetCallerID",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
public  async Task<System.String> GetCallerIDAsync (InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
return await Task.Run(() =>{
return GetCallerID(invokeOption);});
}

}
}
