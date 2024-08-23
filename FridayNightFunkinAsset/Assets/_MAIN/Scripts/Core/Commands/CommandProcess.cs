using System;
using UnityEngine.Events;

namespace COMMANDS
{
    public class CommandProcess
    {
        public Guid ID;
        public string processName;
        public Delegate command;
        public CoroutineWrapper runningProcess;
        public string[] args;

        public UnityEvent onTerminateAction;

        public CommandProcess(Guid ID, string processName, Delegate command, CoroutineWrapper runningProcess, string[] args, UnityEvent onTerminateAction = null)
        {
            this.ID = ID;
            this.processName = processName;
            this.command = command;
            this.runningProcess = runningProcess;
            this.args = args;
            this.onTerminateAction = onTerminateAction;
        }
    }
}