import type { SVGProps } from 'react'

type IconProps = SVGProps<SVGSVGElement>

const base = (props: IconProps) => ({
  width: 16,
  height: 16,
  viewBox: '0 0 16 16',
  fill: 'none',
  stroke: 'currentColor',
  strokeWidth: 1.5,
  strokeLinecap: 'round' as const,
  strokeLinejoin: 'round' as const,
  ...props,
})

export function SearchIcon(props: IconProps) {
  return (
    <svg {...base(props)}>
      <circle cx="7" cy="7" r="4.5" />
      <path d="M13.5 13.5 10.5 10.5" />
    </svg>
  )
}

export function EyeIcon(props: IconProps) {
  return (
    <svg {...base(props)}>
      <path d="M1 8s2.5-5 7-5 7 5 7 5-2.5 5-7 5-7-5-7-5Z" />
      <circle cx="8" cy="8" r="2" />
    </svg>
  )
}

export function PencilIcon(props: IconProps) {
  return (
    <svg {...base(props)}>
      <path d="M11 2.5 13.5 5 5 13.5H2.5V11L11 2.5Z" />
    </svg>
  )
}

export function TrashIcon(props: IconProps) {
  return (
    <svg {...base(props)}>
      <path d="M2.5 4.5h11" />
      <path d="M5.5 4.5V3a1 1 0 0 1 1-1h3a1 1 0 0 1 1 1v1.5" />
      <path d="M4 4.5 4.6 13a1 1 0 0 0 1 .9h4.8a1 1 0 0 0 1-.9l.6-8.5" />
    </svg>
  )
}

export function PlusIcon(props: IconProps) {
  return (
    <svg {...base(props)}>
      <path d="M8 2.5v11" />
      <path d="M2.5 8h11" />
    </svg>
  )
}

export function ChevronDownIcon(props: IconProps) {
  return (
    <svg {...base(props)}>
      <path d="M3.5 5.5 8 10l4.5-4.5" />
    </svg>
  )
}

export function CloseIcon(props: IconProps) {
  return (
    <svg {...base(props)}>
      <path d="M3.5 3.5l9 9" />
      <path d="M12.5 3.5l-9 9" />
    </svg>
  )
}

export function ArrowRightIcon(props: IconProps) {
  return (
    <svg {...base(props)}>
      <path d="M3 8h10" />
      <path d="M9 4l4 4-4 4" />
    </svg>
  )
}

export function ArrowLeftIcon(props: IconProps) {
  return (
    <svg {...base(props)}>
      <path d="M2.5 8h7" />
      <path d="M7 4L2.5 8 7 12" />
    </svg>
  )
}

export function SortIcon({ direction, ...props }: IconProps & { direction?: 'Ascending' | 'Descending' }) {
  if (!direction) {
    return (
      <svg {...base(props)} strokeWidth={1.25} opacity={0.5}>
        <path d="M5 6.5 8 3.5l3 3" />
        <path d="M5 9.5 8 12.5l3-3" />
      </svg>
    )
  }

  return (
    <svg {...base(props)}>
      {direction === 'Ascending' ? <path d="M5 9.5 8 6.5l3 3" /> : <path d="M5 6.5 8 9.5l3-3" />}
    </svg>
  )
}

export function CheckIcon(props: IconProps) {
  return (
    <svg {...base(props)}>
      <path d="M3 8.5 6.2 11.5 13 4.5" />
    </svg>
  )
}
