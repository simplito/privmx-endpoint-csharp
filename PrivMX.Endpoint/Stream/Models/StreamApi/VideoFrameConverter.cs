//
// PrivMX Endpoint C#
// Copyright © 2024 Simplito sp. z o.o.
//
// This file is part of the PrivMX Platform (https://privmx.dev).
// This software is Licensed under the MIT License.
//
// See the License for the specific language governing permissions and
// limitations under the License.
//

#if ANDROID

using System;
using Org.Webrtc;

public static class VideoFrameConverter
{
    public static byte[] ToRGBA(VideoFrame frame)
    {
        //frame.Buffer.BufferType
        //var buffer3 = frame.Buffer as VideoFrame.IBuffer;
        //var buffer2 = frame.Buffer as VideoFrame.ITextureBuffer;
        var buffer = frame.Buffer as VideoFrame.II420Buffer;
        int width = frame.RotatedWidth;
        int height = frame.RotatedHeight;
        byte[] rgba = new byte[width * height * 4];

        if (buffer != null)
        {
            rgba = II420BufferToRGBA(buffer);
        }

        return rgba;
    }
    
    private static byte[] II420BufferToRGBA(VideoFrame.II420Buffer buffer)
    {
        int width = buffer.Width;
        int height = buffer.Height;
        byte[] rgba = new byte[width * height * 4];

        var yBuffer = buffer.DataY;
        var uBuffer = buffer.DataU;
        var vBuffer = buffer.DataV;

        int strideY = buffer.StrideY;
        int strideU = buffer.StrideU;
        int strideV = buffer.StrideV;
        
        yBuffer.Rewind();
        uBuffer.Rewind();
        vBuffer.Rewind();

        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                int yIndex = row * strideY + col;
                int uvRow = row / 2;
                int uvCol = col / 2;
                int uIndex = uvRow * strideU + uvCol;
                int vIndex = uvRow * strideV + uvCol;

                sbyte Y = yBuffer.Get(yIndex);
                sbyte U = uBuffer.Get(uIndex);
                sbyte V = vBuffer.Get(vIndex);

                // YUV -> RGB conversion
                int C = Y - 16;
                int D = U - 128;
                int E = V - 128;

                int R = (298 * C + 409 * E + 128) >> 8;
                int G = (298 * C - 100 * D - 208 * E + 128) >> 8;
                int B = (298 * C + 516 * D + 128) >> 8;

                R = Math.Clamp(R, 0, 255);
                G = Math.Clamp(G, 0, 255);
                B = Math.Clamp(B, 0, 255);

                int rgbaIndex = (row * width + col) * 4;
                rgba[rgbaIndex + 0] = (byte)R;
                rgba[rgbaIndex + 1] = (byte)G;
                rgba[rgbaIndex + 2] = (byte)B;
                rgba[rgbaIndex + 3] = 255;
            }
        }

        return rgba;
    }
}

#endif