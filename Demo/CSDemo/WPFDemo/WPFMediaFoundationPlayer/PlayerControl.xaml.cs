﻿/*
MIT License

Copyright(c) 2020 Evgeny Pereguda

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files(the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions :

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using MediaFoundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFMediaFoundationPlayer
{
    /// <summary>
    /// Interaction logic for PlayerControl.xaml
    /// </summary>
    public partial class PlayerControl : UserControl
    {

        public IntPtr SurfaceNativeInterface { get { return mEVRDisplay.Surface.NativeInterface; } }

        public Interop.Direct3DSurface9 Surface { get { return mEVRDisplay.Surface; } }

        List<Player> lPlayerList = new List<Player>();

        public PlayerControl()
        {
            InitializeComponent();
        }

        public void setRenderList(
            List<IMFTopologyNode> aEVRList,
            CaptureManagerToCSharpProxy.Interfaces.IEVRStreamControl aIEVRStreamControl,
            uint aMaxVideoRenderStreamCount)
        {
            m_PlayerCanvas.Children.Clear();

            foreach (var item in lPlayerList)
            {
                item.Stop();
            }
            
            lPlayerList.Clear();

            int lColumnCount = 2;

            double lRowCount = Math.Ceiling((((double)aEVRList.Count / ((double)lColumnCount + 1.0)) + 1.0));

            double lRowHeight = m_PlayerCanvas.Height / lRowCount;

            double lColumnWidth = m_PlayerCanvas.Width / lColumnCount;

            for (int i = 0; i < aEVRList.Count; i++)
            {
                Player lPlayer = new Player();

                m_PlayerCanvas.Children.Add(lPlayer);

                lPlayerList.Add(lPlayer);

                lPlayer.Width = lColumnWidth;

                lPlayer.Height = lRowHeight;

                int lTopPos = i / (lColumnCount);

                Canvas.SetTop(lPlayer, lTopPos * lRowHeight);

                double lLeftPos = i - (lTopPos * (lColumnCount));

                Canvas.SetLeft(lPlayer, lLeftPos * lColumnWidth);

                lPlayer.mIMFTopologyNode = aEVRList[i];

                lPlayer.mIEVRStreamControl = aIEVRStreamControl;

                lPlayer.mMaxVideoRenderStreamCount = aMaxVideoRenderStreamCount;

                aIEVRStreamControl.setPosition(
                    lPlayer.mIMFTopologyNode,
                    (float)(((double)lLeftPos * lColumnWidth) / m_PlayerCanvas.Width),
                    (float)((((double)lLeftPos * lColumnWidth) + lColumnWidth) / m_PlayerCanvas.Width),
                    (float)(((double)lTopPos * lRowHeight) / m_PlayerCanvas.Height),
                    (float)((((double)lTopPos * lRowHeight) + lRowHeight) / m_PlayerCanvas.Height));
                
                Canvas.SetZIndex(lPlayer, i);
            }
        }
    }
}
