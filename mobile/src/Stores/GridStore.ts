import {
  action, configure, makeAutoObservable, observable, runInAction,
} from 'mobx'
import Grid from '../Tools/Grid'
import {Size} from '../Types/Block'
import {Widget} from '../Types/Widgets'
import {RootStore} from './RootStore'

configure({enforceActions: 'always'})

export class GridStore {
  @observable private _modifying = false;

  @observable private _adding = false;

  @observable private _blocks: Array<Widget> = [];

  @observable private _timePickerVisible = false;
  @observable private _timePickerModifyer: number | undefined;

  constructor(private _rootStore: RootStore) {
    makeAutoObservable(this)
    this.initGrid()
  }

  @action
  public toggleEditionMode = (): void => {
    this._modifying = !this._modifying
  };

  @action
  public toggleAdditionMode = (): void => {
    this._adding = !this._adding
  };

  public get modifying(): boolean {
    return this._modifying
  }

  public get adding(): boolean {
    return this._adding
  }

  private get fillBlock(): any {
    return ({color: '#00000000', unactive: true})
  }

  @action
  private initGrid = async (): Promise<void> => {
    this._rootStore.widget.updateWidgets()
      .then((success) => {
        if (!success) return

        runInAction(() => {
          this._blocks = Grid.fillGrid(
            this._rootStore.widget.widgets,
            this.fillBlock,
            this._rootStore.widget.widgets.length + 4,
          )
        })
      }).catch(console.log)
  };

  public get blocks(): Widget[] {
    return this._blocks
  }

  @action
  public setBlocks = (arr: Widget[]): void => {
    this._blocks = arr
  };

  private addEmptyBlock = (blocks: Widget[], index: number): void => {
    blocks.splice(index, 0, this.fillBlock)
  };

  @action
  private removeAt = (blocks: Widget[], index: number): void => {
    blocks.splice(index, 1)
  };

  @action
  public swithAtIndexes = (index1: number, index2: number, blocks: Widget[] = this._blocks): void => {
    const tmp: Widget = blocks[index1]

    blocks[index1] = blocks[index2]
    blocks[index2] = tmp
  };

  @action
  public setBlockSize = (blockIndex: number, newBlockSize: number): void => {
    if (blockIndex < 0 || blockIndex >= this._blocks.length) {
      return
    }

    const copy: Widget[] = [...this._blocks]

    copy[blockIndex].size = newBlockSize

    for (let i = 0; i < copy.length - 1; ++i) {
      const position: number = Grid.indexToPosition(copy, i)

      if (position % 2 && (copy[i].size ?? Size.normal) !== Size.normal) {
        this.addEmptyBlock(copy, i)
      }
      if (newBlockSize === Size.extended && copy[i + 1].unactive) {
        this.removeAt(copy, i + 1)
        i++
      }
    }

    this._blocks = copy
  };

  public getBlockSize = (blockIndex: number): number => {
    if (blockIndex < 0 || blockIndex >= this._blocks.length) return Size.normal

    return this._blocks[blockIndex]?.size ?? Size.normal
  };

  public isBlockMutable = (blockIndex: number): boolean => {
    if (blockIndex < 0 || blockIndex >= this._blocks.length) return false

    return this._blocks[blockIndex].unactive ? !this._blocks[blockIndex].unactive : this._modifying
  };

  @action
  public modifyRefreshDelay = (hours: number, minutes: number): void => {
    this._timePickerVisible = false


    this._rootStore.widget.setRefreshDelay(this._timePickerModifyer!, hours, minutes)
    /*
    logic
    */

    this._timePickerModifyer = undefined
  }

  @action
  public openTimePicker = (index: number): void => {
    this._timePickerVisible = true
    this._timePickerModifyer = index
  }

  @action
  public closeTimePicker = (): void => {
    this._timePickerVisible = false
    this._timePickerModifyer = undefined
  }

  public isTimePickerVisible = (): boolean => this._timePickerVisible === true
}
