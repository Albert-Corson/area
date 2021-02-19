import {Block} from '../Types/Block'

export default class Grid {
  static switchElem = <T>(
    array: readonly T[],
    index1: number,
    index2: number,
  ): readonly T[] => {
    const copy: T[] = [...array]
    const tmp: T = copy[index1]

    copy[index1] = copy[index2]
    copy[index2] = tmp

    return copy
  };

  static getHoveredBlockIndex = (
    x: number,
    y: number,
    offsetX: number,
    offsetY: number,
  ): number => {
    const xOffsetIndex: number = Math.round(offsetX / 205)
    const yOffsetIndex: number = Math.round(offsetY / 205)

    const xHovered: number = x + xOffsetIndex
    const yHovered: number = y + yOffsetIndex

    return xHovered + yHovered * 2
  };

  static fillGrid = (array: any[], fill: any, to: number): any[] => {
    const offset: number = to - array.length

    if (offset <= 0) {
      return array
    }
    return array.concat(Array(offset).fill(fill))
  };

  static indexToPosition = (array: readonly Block[], index: number): number => {
    const range = Array<number>(index).fill(0).map((_, i) => i)
    let position = 0

    range.forEach((i) => {
      position += array[i].size ?? 1
    })

    return position
  };
}
