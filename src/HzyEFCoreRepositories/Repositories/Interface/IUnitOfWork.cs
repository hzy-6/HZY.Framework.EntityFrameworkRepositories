﻿/*
 * *******************************************************
 *
 * 作者：hzy
 *
 * 开源地址：https://gitee.com/hzy6
 *
 * *******************************************************
 */
namespace HzyEFCoreRepositories.Repositories.Interface
{
    public interface IUnitOfWork
    {
        bool GetSaveState();
        void SetSaveState(bool saveSate);
        void CommitOpen();
    }
}

