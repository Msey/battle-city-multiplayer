#include "widget.h"

#include <QApplication>
#include <QFileDialog>
#include <QImage>
#include <QPainter>
#include <QDebug>

void replaceColors(QImage& image, const QVector<QPair<QColor, QColor>>& colors)
{
    for (int y = 0; y < image.height(); ++y)
        for (int x = 0; x < image.width(); ++x)
        {
            QColor currentColor = image.pixelColor(x, y);
            for (const auto& replaceColor : colors)
            {
                if (replaceColor.first == currentColor)
                {
                    image.setPixelColor(x, y, replaceColor.second);
                    break;
                }
            }
        }
}

int main(int argc, char *argv[])
{
    QApplication a(argc, argv);

    const QString tanksBaseTextureFileName = QFileDialog::getOpenFileName();
    QImage tanksBaseTexture(tanksBaseTextureFileName);
    if (tanksBaseTexture.isNull())
    {
        qDebug() << "empty file";
        return -1;
    }

    QVector<QPair<QColor, QColor>> replaceColorsTable;

    //yellow
    //replaceColorsTable.push_back(qMakePair(QColor(46, 106, 66), QColor(107, 107, 0))); //dark color
    //replaceColorsTable.push_back(qMakePair(QColor(80, 155, 75), QColor(231, 156, 33))); //light color

    //blue
//    replaceColorsTable.push_back(qMakePair(QColor(46, 106, 66), QColor(0, 66, 74))); //dark color
//    replaceColorsTable.push_back(qMakePair(QColor(80, 155, 75), QColor(173, 173, 173))); //light color

    //green
   // replaceColorsTable.push_back(qMakePair(QColor(46, 106, 66), QColor(0, 82, 0))); //dark color
   // replaceColorsTable.push_back(qMakePair(QColor(80, 155, 75), QColor(0, 140, 49))); //light color

    //bonus
    replaceColorsTable.push_back(qMakePair(QColor(46, 106, 66), QColor(90, 0, 123))); //dark color
    replaceColorsTable.push_back(qMakePair(QColor(80, 155, 75), QColor(181, 49, 33))); //light color

    QImage resultAtlas(128, 128, QImage::Format_ARGB32);
    resultAtlas.fill(Qt::transparent);
    {
        QPainter painter(&resultAtlas);
        for (int tankIndex = 0; tankIndex < 8; ++tankIndex)
        {
            for (int rotationIndex = 0; rotationIndex < 8; ++rotationIndex)
            {
                const bool copyFirstTankTexture = rotationIndex % 2;
                QImage tankBase = tanksBaseTexture.copy(copyFirstTankTexture ? 0 : 16, 16 * tankIndex, 16, 16);
                replaceColors(tankBase, replaceColorsTable);

                int rotationSide = rotationIndex / 2;
                switch (rotationSide) {
                    case 1:
                    {
                        tankBase = tankBase.transformed(QMatrix().rotate(-90.0));
                        break;
                    }
                    case 2:
                    {
                        tankBase = tankBase.mirrored(false, true);
                        break;
                    }
                    case 3:
                    {
                        tankBase = tankBase.transformed(QMatrix().rotate(-90.0)).mirrored(true, false);
                        break;
                    }
                }

                painter.save();
                painter.drawImage(rotationIndex * 16, tankIndex * 16, tankBase);
                painter.restore();
            }
        }
    }

    resultAtlas.save("atlas4.png");

    qDebug() << "succes";
    return 0;
}
