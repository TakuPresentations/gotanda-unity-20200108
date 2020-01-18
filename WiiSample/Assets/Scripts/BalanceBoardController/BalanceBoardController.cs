﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceBoardController : SingletonBehaviour<BalanceBoardController>
{
    public int? connectingBalanceBoardIndex { get; private set; }

    public bool IsRide { get; private set; }

    // 総重量に対して指定した割合以上の差分がある場合、前後に進ませる
    [SerializeField] private float TriggerThreasoldRate = 0.2f;
    // この重量以下なら体重計に乗っていないと断定
    [SerializeField] private float RideThreasoldWeight = 15f;

    private float ResetGameTimeSecond = 0f;

    public Action<Vector2, Vector4> OnRidingWeightSreeem = null;

    void Start()
    {
        connectingBalanceBoardIndex = GetBalanceBoardConnectingIndex();
    }

    void Update()
    {
        for (int i = 0; i < Wii.GetRemoteCount(); ++i)
        {
            if (Wii.IsActive(i) && Wii.GetExpType(i) == 3)
            {
                Vector4 theBalanceBoard = Wii.GetBalanceBoard(i);
                Vector2 theCenter = Wii.GetCenterOfBalance(i);
                float topWeight = theBalanceBoard.x + theBalanceBoard.y;
                float bottomWeight = theBalanceBoard.z + theBalanceBoard.w;
                float totalWeight = Wii.GetTotalWeight(i);
                float topBottomDiff = topWeight - bottomWeight;
                bool beforeIsRide = IsRide;
                IsRide = totalWeight > RideThreasoldWeight;
                if(IsRide){
                    if(OnRidingWeightSreeem != null)
                    {
                        OnRidingWeightSreeem(theCenter, theBalanceBoard);
                    }
                    ResetGameTimeSecond = 0f;
                    if (Mathf.Abs(topBottomDiff) > (totalWeight * TriggerThreasoldRate))
                    {
                        if(topBottomDiff > 0){
                        }else{
                        }
                    }
                    else
                    {
                    }
                }
                break;
            }
        }
    }

    // WiiFitに接続している番号を保持して処理回数を減らす
    public int? GetBalanceBoardConnectingIndex()
    {
        for (int i = 0; i < Wii.GetRemoteCount(); ++i)
        {
            // GetExpType == 3 がBaranceBoard
            if (Wii.IsActive(i) && Wii.GetExpType(i) == 3)
            {
                return i;
            }
        }
        return null;
    }

    // WiiFitに接続しているかどうか判定
    public bool IsBalanceBoardConnecting()
    {
        for (int i = 0; i < Wii.GetRemoteCount(); ++i)
        {
            // GetExpType == 3 がBaranceBoard
            if (Wii.IsActive(i) && Wii.GetExpType(i) == 3)
            {
                return true;
            }
        }
        return false;
    }

    // WiiFitのバッテリー残量
    public float GetBalanceBoardBattery()
    {
        for (int i = 0; i < Wii.GetRemoteCount(); ++i)
        {
            // GetExpType == 3 がBaranceBoard
            if (Wii.IsActive(i) && Wii.GetExpType(i) == 3)
            {
                return Wii.GetBattery(i);
            }
        }
        return 0f;
    }
}
