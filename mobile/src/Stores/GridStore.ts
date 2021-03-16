import {
  action, configure, makeAutoObservable, observable, runInAction,
} from 'mobx'
import Grid from '../Tools/Grid'
import {Size} from '../Types/Block'
import {Widget} from '../Types/Widgets'
import {RootStore} from './RootStore'
import AsyncStorage from '@react-native-async-storage/async-storage'
import {block} from 'react-native-reanimated'

configure({enforceActions: 'always'})

export class GridStore {
  @observable private _modifying = false;

  @observable private _adding = false;

  @observable private _blocks: Array<Widget> = [];

  @observable private _timePickerVisible = false;

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

  @action
  public set adding(value: boolean) {
    this._adding = value
  }

  private get fillBlock(): any {
    return ({color: '#00000000', unactive: true})
  }

  @action
  public initGrid = async (): Promise<void> => {
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

  @action
  private parseWidgetDisplayItems = (item: Widget): any => {
    if (item?.params?.items?.length) {
      const max = item.params?.items?.length

      if (item.currentParam == undefined) {
        item.currentParam = ~~(Math.random() * (max))
      }

      return item.params.items[item.currentParam]
    }
    return {}
  }

  @action
  public get blocks(): Widget[] {
    return this._blocks.map(action((widget) => ({...widget, display: this.parseWidgetDisplayItems(widget)})))
  }

  @action
  public setBlocks = (arr: Widget[]): void => {
    this._blocks = arr.map(block => ({...block, currentParam: undefined}))

    this.applyProfile()
  };
  
  @action
  public setBlock = (widget: Widget): void => {
    const old = this._blocks.filter((item) => widget.id === item.id)[0]
    const index = this._blocks.indexOf(old)
    const copy = [...this._blocks]

    copy[index] = {...old, ...widget, currentParam: undefined}

    this._blocks = copy

    this.applyProfile()
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

    this.saveGridProfile()
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

    this.saveGridProfile()
  };

  public getBlockSize = (blockIndex: number): number => {
    if (blockIndex < 0 || blockIndex >= this._blocks.length) return Size.normal

    return this._blocks[blockIndex]?.size ?? Size.normal
  };

  public isBlockMutable = (blockIndex: number): boolean => {
    console.log('index: ', blockIndex)
    console.log('length: ', this._blocks.length)
    console.log('blocks: ', this._blocks.map(block => ({name: block.name,})))
    if (blockIndex < 0 || blockIndex >= this._blocks.length) return false

    return this._blocks[blockIndex] && this._blocks[blockIndex].unactive ? !this._blocks[blockIndex].unactive : this._modifying
  };

  public openTimePicker = (): void => {
    this._timePickerVisible = true
  }

  public closeTimePicker = (): void => {
    this._timePickerVisible = false
  }

  public isTimePickerVisible = (): boolean => this._timePickerVisible === true

  private saveGridProfile = (): void => {
    console.log(this._rootStore.user?.user)
    if (this._rootStore.user?.user?.id == null) return 

    const profile: Record<number, Record<string, any>> = {}

    this._blocks.map((block, index) => {
      profile[block.id] = {
        size: block.size || Size.normal,
        index,
      }
    })

    AsyncStorage.setItem(`@profile_${this._rootStore.user.user.id || -1}`, JSON.stringify(profile))
  }

  @action
  private applyProfile = async () => {
    if (this._rootStore.user?.user?.id == null) return

    try {
      const profile = await AsyncStorage.getItem(`@profile_${this._rootStore.user.user.id}`)

      if (profile) {
        const object = JSON.parse(profile)

        runInAction(() => {
          const copy = [...this._blocks].map((block) => {
            return {
              ...block,
              size: object[block.id] ? object[block.id].size : Size.normal,
            }
          })

          const final: Widget[] = []

          Object.keys(object).map((key, index) => {
            final[object[key].index] = copy[index]
          })

          this._blocks = final
        })
      }
    } catch (e) {
      console.warn(e)
    }
  }
}
