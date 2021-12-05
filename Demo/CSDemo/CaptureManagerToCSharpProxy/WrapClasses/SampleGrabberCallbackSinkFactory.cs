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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaptureManagerToCSharpProxy.Interfaces;

namespace CaptureManagerToCSharpProxy.WrapClasses
{
    class SampleGrabberCallbackSinkFactory : ISampleGrabberCallbackSinkFactory, ISampleGrabberCallbackSinkFactoryAsync
    {
        private CaptureManagerLibrary.ISampleGrabberCallbackSinkFactory mISampleGrabberCallbackSinkFactory;

        public SampleGrabberCallbackSinkFactory(CaptureManagerLibrary.ISampleGrabberCallbackSinkFactory aISampleGrabberCallSinkFactory)
        {
            mISampleGrabberCallbackSinkFactory = aISampleGrabberCallSinkFactory;
        }
        
        private async Task<ISampleGrabberCallback> createOutputNodeTask(
            Guid aRefMajorType,
            Guid aRefSubType,
            bool aIsAwait)
        {
            return await Task.Run(() =>
            {
                ISampleGrabberCallback lresult = null;

                do
                {
                    if (mISampleGrabberCallbackSinkFactory == null)
                        break;


                    try
                    {
                        SampleGrabberCallback lCallback = new SampleGrabberCallback();

                        object lIUnknown;

                        mISampleGrabberCallbackSinkFactory.createOutputNode(
                            aRefMajorType,
                            aRefSubType,
                            lCallback,
                            out lIUnknown);
                        
                        if (lIUnknown == null)
                            break;

                        lCallback.TopologyNode = lIUnknown;

                        lresult = lCallback;
                    }
                    catch (Exception exc)
                    {
                        LogManager.getInstance().write(exc.Message);
                    }

                } while (false);

                return lresult;
            }).ConfigureAwait(aIsAwait);
        }

        public bool createOutputNode(
            Guid aRefMajorType, 
            Guid aRefSubType,
            out ISampleGrabberCallback aISampleGrabberCallback)
        {
            bool lresult = false;

            aISampleGrabberCallback = createOutputNodeTask(aRefMajorType, aRefSubType, false).Result;

            lresult = aISampleGrabberCallback != null;

            return lresult;
        }

        public async Task<ISampleGrabberCallback> createOutputNodeAsync(Guid aRefMajorType, Guid aRefSubType)
        {
            return await createOutputNodeTask(aRefMajorType, aRefSubType, true);
        }
    }
}